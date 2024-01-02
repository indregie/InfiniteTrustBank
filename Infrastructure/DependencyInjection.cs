using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        //services.AddTransient<ITodoRepository, TodoRepository>();
        //services.AddHttpClient();
    }
}
