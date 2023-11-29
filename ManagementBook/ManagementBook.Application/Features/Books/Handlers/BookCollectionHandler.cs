﻿namespace ManagementBook.Application.Features.Books.Handlers;

using AutoMapper;
using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Queries;
using ManagementBook.Domain.Books;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

public class BookCollectionHandler : IRequestHandler<BookCollectionQuery, Result<IQueryable<Book>>>
{
    private IMapper _mapper;
    private IBookRepository _bookRepository;

    public BookCollectionHandler(IMapper mapper, IBookRepository bookRepository)
    {
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public async Task<Result<IQueryable<Book>>> Handle(BookCollectionQuery request, CancellationToken cancellationToken)
    => await TryAsync(
        async () => new Result<IQueryable<Book>>(await _bookRepository.GetAll())
    ).IfFail(fail => new Result<IQueryable<Book>>(fail));
}
