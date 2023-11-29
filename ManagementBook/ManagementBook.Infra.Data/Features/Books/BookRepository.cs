namespace ManagementBook.Infra.Data.Features.Books;

using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Data.Base;
using System;
using System.Threading.Tasks;

public class BookRepository : IBookRepository
{
    private BookStoreContext _baseContext;

    public BookRepository(BookStoreContext baseContext)
    {
        _baseContext = baseContext;
    }

    public Task<Result<Book>> Get(Predicate<Book> predicate)
    {
        throw new NotImplementedException();
    }

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
