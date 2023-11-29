namespace ManagementBook.Infra.Data.Base.Designs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

internal class BookStoreContextFactoryDesignTime : IDesignTimeDbContextFactory<BookStoreContext>
{
    private string _localConnectionString = @"Server=172.19.0.2;Database=BookStore;User ID=sa;Password=B00kSt0r3;Trusted_Connection=true;";
    public BookStoreContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BookStoreContext>();
        builder.UseSqlServer(_localConnectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

        return new BookStoreContext(builder.Options);
    }
}
