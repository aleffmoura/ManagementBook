namespace ManagementBook.Domain.Books;

using LanguageExt;
using LanguageExt.Common;
using System.Collections.Generic;

public interface IBookRepository
{
    Task<IQueryable<Book>> GetAll();
    Task<Result<Book>> Get(Predicate<Book> predicate);
    Task<Result<Unit>> Save(Option<Book> book);
    Task<Result<Unit>> Update(Option<Book> book);
    Task<Result<Unit>> Remove(Guid guid);
}
