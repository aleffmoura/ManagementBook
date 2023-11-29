namespace ManagementBook.Infra.Data.Tests.Books;

using FluentAssertions;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Data.Base;
using ManagementBook.Infra.Data.Features.Books;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Data.SqlTypes;
using System.Threading.Tasks;

[TestFixture]
public class BookRepositoryTests
{
    private Mock<BookStoreContext> _bookStoreMock;
    private BookRepository _bookRepository;

    [SetUp]
    public void SetUp()
    {
        _bookStoreMock = new ();
        _bookRepository = new (_bookStoreMock.Object);
    }

    [Test]
    public async Task BookRepositoryTests_GetAll_ShouldBeOk()
    {
        //arrange

        var booksCount = 1;
        List<Book> booksOnDb =
        [
            new Book
            {
                Id = Guid.NewGuid(),
                Author = "Author",
                Title = "Title",
                ReleaseDate = DateTime.Now,
            }
        ];
        _bookStoreMock.Setup(bs => bs.AsNoTracking(It.IsAny<IQueryable<Book>>()))
                      .Returns(booksOnDb.AsQueryable())
                      .Verifiable();

        var dbSetMock = new Mock<DbSet<Book>>();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var result = await _bookRepository.GetAll();

        //verifies
        result.Count().Should().Be(expected: booksCount);
        dbSetMock.Verify();
        _bookStoreMock.Verify();
    }

    [Test]
    public async Task BookRepositoryTests_GetAll_ShouldBeThrowNullReferenceException()
    {
        //action
        var action = async () => await _bookRepository.GetAll();

        //verifies
        await action.Should().ThrowAsync<NullReferenceException>();

        _bookStoreMock.Verify();
    }

    [Test]
    public async Task BookRepositoryTests_GetAll_ShouldBeSqlTruncateException()
    {
        //arrange
        _bookStoreMock.Setup(bs => bs.AsNoTracking(It.IsAny<IQueryable<Book>>()))
                      .Throws<SqlTruncateException>()
                      .Verifiable();

        var dbSetMock = new Mock<DbSet<Book>>();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var action = async () => await _bookRepository.GetAll();


        //verifies
        await action.Should().ThrowAsync<SqlTruncateException>();
        dbSetMock.Verify();
        _bookStoreMock.Verify();
    }
}
