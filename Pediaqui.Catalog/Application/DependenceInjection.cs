using Application.Features.ClienteContext;
using Application.Features.ProdutoContext;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddScoped<NotificationContext>();

        services.AddScoped<IProdutoPresenter, ProdutoPresenter>();
        services.AddScoped<IClientePresenter, ClientePresenter>();

        return services;
    }
}
