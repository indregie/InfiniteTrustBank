using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddTransient<UserService>();
        services.AddTransient<AccountService>();
        services.AddTransient<TransactionService>();
    }
}
