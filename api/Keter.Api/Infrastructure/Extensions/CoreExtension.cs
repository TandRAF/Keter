// Keter.Api/Infrastructure/Extensions/CoreExtensions.cs
namespace Keter.Api.Infrastructure.Extensions;

public static class CoreExtensions
{
    public static IServiceCollection AddVerticalSliceCore(this IServiceCollection services)
    {
        // Add Controllers and override the default JSON parser with Newtonsoft
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                // Example: Ignore circular references (crucial for Entity Framework!)
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                // Example: Format dates cleanly
                options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
            });

        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        return services;
    }
}