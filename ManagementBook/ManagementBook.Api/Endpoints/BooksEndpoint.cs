namespace ManagementBook.Api.Endpoints;

using AutoMapper;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Api.DTOs;
using ManagementBook.Api.ViewModels;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Application.Features.Books.Queries;
using ManagementBook.Domain.Books;
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
                   async ([FromServices] IMediator mediator,
                          [FromServices] IMapper mapper) =>
                   {
                       var returned = await mediator.Send(new BookCollectionQuery());

                       return HandleQueryable<Book, BookDetailViewModel>(returned, mapper);
                   }
        ).WithName($"Get{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }

    public static WebApplication BookGetByIdEndpoint(this WebApplication app)
    {
        app.MapGet($"{_baseEndpoint}/{{id}}",
                   async ([FromServices] IMediator mediator,
                          [FromServices] IMapper mapper,
                          [FromRoute] Guid id) =>
                   {
                       var returned = await mediator.Send(new BookByIdQuery(id));

                       return HandleQuery<Book, BookDetailViewModel>(returned, mapper);
                   }
        ).WithName($"GetById{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }

    public static WebApplication BookPostEndpoint(this WebApplication app)
    {
        app.MapPost($"{_baseEndpoint}",
                   async ([FromServices] IMediator mediator,
                          [FromServices] IMapper mapper,
                          [FromBody] BookCreateDto createDto) =>
                   {
                       return HandleCommand(await mediator.Send(mapper.Map<BookSaveCommand>(createDto)));
                   }
        ).WithName($"Post{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }

    public static WebApplication BookPatchEndpoint(this WebApplication app)
    {
        app.MapPatch($"{_baseEndpoint}/{{id}}",
                   async ([FromServices] IMediator mediator,
                          [FromServices] IMapper mapper,
                          [FromRoute] Guid id,
                          [FromBody] BookUpdateDto updateDto) =>
                   {
                       return HandleCommand(await mediator.Send(mapper.Map<BookUpdateCommand>((id, updateDto))));
                   }
        ).WithName($"Patch{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }

    #region Private Methods
    private static IResult HandleCommand<TSource>(Result<TSource> result)
        => result.Match(succ => Results.Ok(succ), error => HandleFailure(error));

    private static IResult HandleQuery<TSource, TDestiny>(Result<TSource> result, IMapper m)
        => result.Match(succ => Results.Ok(m.Map<TDestiny>(succ)), error => HandleFailure(error));
    private static IResult HandleQueryable<TSource, TDestiny>(Result<IQueryable<TSource>> result, IMapper m)
        => result.Match(succ => Results.Ok(m.ProjectTo<TDestiny>(succ, m.ConfigurationProvider)), error => HandleFailure(error));

    private static IResult HandleFailure<T>(T exception) where T : Exception
        => exception is ValidationException validationError
            ? Results.Problem(detail: JsonSerializer.Serialize(validationError.Errors), statusCode: HttpStatusCode.BadRequest.GetHashCode())
            : ErrorPayload.New(exception).Apply(error => Results.Problem(detail: JsonSerializer.Serialize(error), statusCode: error.ErrorCode.GetHashCode()));
    #endregion
}
