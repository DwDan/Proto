using System.Linq.Dynamic.Core;

namespace Proto.Common.Infrastructure;

public static class OrderParser
{
    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
            return query;

        var orderParts = order.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var orderField = orderParts[0];
        var orderDirection = orderParts.Length > 1 ? orderParts[1].ToLower() : "asc";

        var validProperties = typeof(T).GetProperties()
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (!validProperties.Contains(orderField))
            return query;

        var normalizedDirection = orderDirection == "desc" ? "descending" : "";
        var fullOrder = $"{orderField} {normalizedDirection}".Trim();

        return query.OrderBy(fullOrder);
    }
}