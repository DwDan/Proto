using FluentValidation;

namespace Proto.Common.Domain.Validation;

public class OrderValidator<T> : AbstractValidator<string>
{
    public OrderValidator()
    {
        When(order => !string.IsNullOrWhiteSpace(order), () =>
        {
            RuleFor(order => order!)
                .Must(IsValidOrderBy)
                .WithMessage(order=> $"Format order '{order}' is not valid. Ensure correct column names and format (e.g., 'price desc, title asc').");
        });
    }

    private bool IsValidOrderBy(string order)
    {
        var validProperties = typeof(T).GetProperties().Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var orderParams = order.Split(',');

        foreach (var param in orderParams)
        {
            var trimmedParam = param.Trim();
            var isDescending = trimmedParam.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
            var propertyName = trimmedParam.Split(' ')[0];

            if (!validProperties.Contains(propertyName))
                return false;
        }

        return true;
    }
}