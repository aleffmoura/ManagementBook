namespace ManagementBook.Api.Extensions;

using ManagementBook.Application;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Data.Base;
using ManagementBook.Infra.Data.Features.Books;
using Microsoft.EntityFrameworkCore;

public static class ServicesExt
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationAssembly>());
        services.AddDbContext<BookStoreContext>(opt => opt.UseSqlServer("", o => o.CommandTimeout(commandTimeout: 10)));
        services.AddScoped<IBookRepository, BookRepository>();

        return services;
    }
}
