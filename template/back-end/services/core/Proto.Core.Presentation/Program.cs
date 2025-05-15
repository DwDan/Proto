using Microsoft.EntityFrameworkCore;
using Proto.Common.Presentation;
using Proto.Core.Infrastructure;
using Proto.Core.IoC;
using Serilog;

public partial class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            var app = CreateWebApplication(args);

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.RegisterDependencies();

        builder.Services.AddAutoMapper(typeof(Program).Assembly);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ValidationExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseCors("AllowFrontend");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();

            dbContext.Database.Migrate();
        }

        return app;
    }
}