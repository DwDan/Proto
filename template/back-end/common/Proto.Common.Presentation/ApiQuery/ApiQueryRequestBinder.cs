using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Proto.Common.WebApi;

public class ApiQueryRequestBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var query = bindingContext.HttpContext.Request.Query;

        var result = new ApiQueryRequestPresentation
        {
            Page = query.TryGetValue(nameof(ApiQueryRequestPresentation.Page), out var pageValue) && int.TryParse(pageValue, out var page) ? page : 1,
            Size = query.TryGetValue(nameof(ApiQueryRequestPresentation.Size), out var sizeValue) && int.TryParse(sizeValue, out var size) ? size : 10,
            Order = query.TryGetValue(nameof(ApiQueryRequestPresentation.Order), out var orderValue) ? orderValue.ToString() : string.Empty,
            Filters = new Dictionary<string, string>()
        };

        var fixedParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            nameof(ApiQueryRequestPresentation.Page),
            nameof(ApiQueryRequestPresentation.Size),
            nameof(ApiQueryRequestPresentation.Order)
        };

        result.Filters = query
            .Where(q => !fixedParams.Contains(q.Key))
            .ToDictionary(q => q.Key, q => q.Value.ToString());

        bindingContext.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }
}
