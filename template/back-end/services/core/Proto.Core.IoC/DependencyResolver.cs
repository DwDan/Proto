using Proto.Core.IoC.ModuleInitializers;
using Microsoft.AspNetCore.Builder;

namespace Proto.Core.IoC;

public static class DependencyResolver
{
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        new InfrastructureModuleInitializer().Initialize(builder);
        new WebApiModuleInitializer().Initialize(builder);
        new ApplicationModuleInitializer().Initialize(builder);
    }
}