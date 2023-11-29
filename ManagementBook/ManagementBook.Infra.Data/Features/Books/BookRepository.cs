namespace ManagementBook.Infra.Data.Features.Books;

using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using ManagementBook.Infra.Data.Base;
using System;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

public class BookRepository : IBookRepository
{
    private BookStoreContext _baseContext;

    public BookRepository(BookStoreContext baseContext)
    {
        _baseContext = baseContext;
    }

    public async Task<Result<Book>> GetById(Guid id)
        => await TryAsync(async () =>
             ( await _baseContext.Books.FindAsync(id) )
                    .Apply(book => book is null
                        ? new Result<Book>(new NotFoundError($"Book with {{id}}: {id} not found."))
                        : book))
        .IfFail(fail => new Result<Book>(new InternalError("Error on GetById, contact the admin.", fail)));

    public Task<IQueryable<Book>> GetAll()
        => _baseContext.AsNoTracking(_baseContext.Books.AsQueryable()).AsTask();

    public Task<Result<Unit>> Remove(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Unit>> Save(Option<Book> book)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Unit>> Update(Option<Book> book)
    {
        throw new NotImplementedException();
    }
}
