using Microsoft.AspNetCore.Builder;

namespace Proto.Core.IoC;

public interface IModuleInitializer
{
    void Initialize(WebApplicationBuilder builder);
}
