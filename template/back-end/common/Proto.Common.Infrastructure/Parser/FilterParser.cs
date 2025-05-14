using System.Linq.Dynamic.Core;

namespace Proto.Common.Infrastructure;

public static class FilterParser
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, Dictionary<string, string> filters)
    {
        if (filters == null || filters.Count == 0)
            return query;

        var whereClauses = new List<string>();
        var parameters = new List<object>();
        var entityType = typeof(T);
        var properties = entityType.GetProperties();
        var propertyMap = properties.ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        int paramIndex = 0;

        foreach (var filter in filters)
        {
            var (columnName, operatorSymbol) = ParseKey(filter.Key);
            if (columnName == null || operatorSymbol == null || !propertyMap.TryGetValue(columnName, out var propertyInfo))
                continue;

            string? clause = BuildWhereClause(entityType, propertyInfo.Name, operatorSymbol, paramIndex, filter.Value, out object? parameter);
            if (clause == null)
                continue;

            whereClauses.Add(clause);
            if (parameter != null)
            {
                parameters.Add(parameter);
                paramIndex++;
            }
        }

        if (!whereClauses.Any())
            return query;

        var whereExpression = string.Join(" AND ", whereClauses);
        return query.Where(whereExpression, parameters.ToArray());
    }

    private static (string? Column, string? Operator) ParseKey(string key)
    {
        var parts = key.Split('-');
        return parts.Length switch
        {
            1 => (parts[0], "eq"),
            2 => (parts[0], parts[1]),
            _ => (null, null)
        };
    }

    private static string? BuildWhereClause(Type entityType, string propertyName, string operatorSymbol, int paramIndex, string value, out object? parameter)
    {
        parameter = null;
        string paramPlaceholder = $"@{paramIndex}";

        var property = entityType.GetProperty(propertyName);
        if (property == null)
            return null;

        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

        try
        {
            return operatorSymbol switch
            {
                "eq" => SetParam($"{propertyName} == {paramPlaceholder}", value, propertyType, out parameter),
                "ne" => SetParam($"{propertyName} != {paramPlaceholder}", value, propertyType, out parameter),
                "gt" => SetParam($"{propertyName} > {paramPlaceholder}", value, propertyType, out parameter),
                "lt" => SetParam($"{propertyName} < {paramPlaceholder}", value, propertyType, out parameter),
                "ge" => SetParam($"{propertyName} >= {paramPlaceholder}", value, propertyType, out parameter),
                "le" => SetParam($"{propertyName} <= {paramPlaceholder}", value, propertyType, out parameter),
                "like" => SetParam($"{propertyName}.Contains({paramPlaceholder})", value, typeof(string), out parameter),
                "startsWith" => SetParam($"{propertyName}.StartsWith({paramPlaceholder})", value, typeof(string), out parameter),
                "endsWith" => SetParam($"{propertyName}.EndsWith({paramPlaceholder})", value, typeof(string), out parameter),
                "isEmpty" => $"string.IsNullOrEmpty({propertyName})",
                "isNotEmpty" => $"!string.IsNullOrEmpty({propertyName})",
                "null" => $"{propertyName} == null",
                "notNull" => $"{propertyName} != null",
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }

    private static string SetParam(string clause, string value, Type targetType, out object? param)
    {
        param = Convert.ChangeType(value, targetType);
        return clause;
    }
}
