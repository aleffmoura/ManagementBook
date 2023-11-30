namespace ManagementBook.Application.Features.Books.Commands;

using LanguageExt.Common;
using MediatR;
using System;
using Unit = LanguageExt.Unit;

public class BookSaveCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
    public string? Author { get; set; }
    public string? Title { get; set; }
    public DateTime Released { get; set; }
}
