namespace Flight.Management.System.API.Extensions;
public static class StartupExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies()
           .Where(p => p.GetName().Name == "Flight.Management.System.API")
           .FirstOrDefault();

        if (assembly != null)
        {
            assembly.GetTypes()
                .Where(p => p.Name.EndsWith("Service"))
                .ToList()
                .ForEach(p =>
                    services.AddScoped(p)
                );
        }
        return services;
    }
}