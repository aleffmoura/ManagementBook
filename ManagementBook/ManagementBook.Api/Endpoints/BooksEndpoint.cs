namespace ManagementBook.Api.Endpoints;

using ManagementBook.Application.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public static class BooksEndpoint
{
    const string _baseEndpoint = "Books";
    public static WebApplication BookGetEndpoint(this WebApplication app)
    {
        app.MapGet(_baseEndpoint,
                   async ([FromServices] IMediator mediator) =>
                   {
                       var returned = await mediator.Send(new BookCollectionQuery());

                       return returned.Match(books => Results.Ok(books),
                                             fail => Results.Problem(detail: JsonSerializer.Serialize(fail), statusCode: 500));
                   }
        ).WithName($"Get{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }
}
