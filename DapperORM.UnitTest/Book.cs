using Dapper;
using DapperORM.App.Database;
using DapperORM.App.Models;
using DapperORM.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DapperORM.UnitTest
{
    [Collection("Sequential")]
    public class BookTest : IClassFixture<ServiceFixture>, IAsyncLifetime
    {
        private readonly IDBContext dbContext;
        private readonly IBookService bookService;
        private readonly IHost testHost;
        private readonly Checkpoint checkpoint = new();

        public BookTest(ServiceFixture fixture)
        {
            testHost = fixture.TestHost;
            dbContext = testHost.Services.GetService<IDBContext>();
            bookService = testHost.Services.GetService<IBookService>();
        }

        [Theory(DisplayName = "Book Crud Test")]
        [ClassData(typeof(BookData))]
        public async void Crud(List<InputBook> newBooks)
        {
            // Create
            InstanceCUDMessage<int> addMessage = await bookService.Save(newBooks);

            Assert.True(addMessage.Ok);
            Assert.Equal(addMessage.NumAffected, newBooks.Count);

            var dbnames = (
                from newBook in newBooks
                select newBook.DBName
            );

            var booksInDB = await bookService.GetByDBName(dbnames);
            Assert.Equal(booksInDB.Count(), newBooks.Count);

            var newRating = 2.2f;

            // Update
            var updatedBooks = (
                from book in booksInDB
                select new InputBook(
                    Id: book.Id,
                    DBName: book.DBName,
                    Title: book.Title,
                    Rating: newRating,
                    UpdateDate: book.UpdateDate,
                    DeleteDate: book.DeleteDate
                )
            );

            var ids = (
                from book in booksInDB
                select book.Id
            );

            var updateMessage = await bookService.Save(updatedBooks);
            Assert.True(updateMessage.Ok);
            Assert.Equal(updateMessage.NumAffected, updatedBooks.Count());

            var booksInDBAfterUpdate = await bookService.GetByID(ids);
            Assert.NotEmpty(booksInDBAfterUpdate);

            foreach (var book in booksInDBAfterUpdate)
            {
                Assert.Equal(book.Rating, newRating);
            }

            // Delete
            var deleteMessage = await bookService.DeleteByID(ids);
            Assert.True(deleteMessage.Ok);
            Assert.Equal(deleteMessage.NumAffected, ids.Count());

            var booksInDBAfterDelete = await bookService.GetByID(ids);
            Assert.Empty(booksInDBAfterDelete);

            // Note: Delete won't remove records in DB
            // It will just update the "DeleteDate" fields to mark as deleted
            // SPs called by GetByDBNames(), GetByIDs() won't get deleted items
            var deletedBooks = await dbContext.Connection.QueryAsync<Book>($@"
                SELECT * FROM [Books] WHERE Id in ({string.Join(',', ids)})
            ");

            Assert.NotEmpty(deletedBooks);
            foreach (var book in deletedBooks)
            {
                Assert.NotNull(book.DeleteDate);
            }

        }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync() => checkpoint.Reset(dbContext.Connection as DbConnection);
    }

    public class BookData : TheoryData<List<InputBook>>
    {
        public BookData()
        {
            Add(new List<InputBook>() {
                new InputBook(
                    DBName: "book-StoryMixups",
                    Title: "Story Mix-ups",
                    Rating: 1.1f
                ),
                new InputBook(
                    DBName: "book-KarloloosAdventure",
                    Title: "Karloloo's Adventure",
                    Rating: 3.1f
                ),
            });
        }

    }
}