using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proto.Core.Infrastructure;

namespace Proto.Core.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Proto.Core.Infrastructure")
            )
        );

        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
    }
}