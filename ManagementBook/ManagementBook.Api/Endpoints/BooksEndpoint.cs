namespace ManagementBook.Api.Endpoints;

using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Queries;
using ManagementBook.Infra.Cross.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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

                       return HandleQueryable(returned);
                   }
        ).WithName($"Get{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }

    private static IResult HandleQueryable<TSource>(Result<TSource> result)
        => result.Match(succ => Results.Ok(succ),
                        error => HandleFailure(error));

    private static IResult HandleFailure<T>(T exception) where T : Exception
        => exception is ValidationException validationError
            ? Results.Problem(detail: JsonSerializer.Serialize(validationError.Errors), statusCode: HttpStatusCode.BadRequest.GetHashCode())
            : ErrorPayload.New(exception).Apply(error => Results.Problem(detail: JsonSerializer.Serialize(error), statusCode: error.ErrorCode.GetHashCode()));
}
