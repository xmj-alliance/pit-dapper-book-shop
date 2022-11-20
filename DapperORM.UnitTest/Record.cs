using DapperORM.App.Database;
using DapperORM.App.Models;
using DapperORM.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DapperORM.UnitTest
{
    [Collection("Sequential")]
    public class RecordTest : IClassFixture<ServiceFixture>, IAsyncLifetime
    {
        private readonly IDBContext dbContext;
        private readonly IBookService bookService;
        private readonly IReaderService readerService;
        private readonly IRecordService recordService;
        private readonly IHost testHost;
        private readonly Checkpoint checkpoint = new ();

        public RecordTest(ServiceFixture fixture)
        {
            testHost = fixture.TestHost;
            dbContext = testHost.Services.GetService<IDBContext>();
            bookService = testHost.Services.GetService<IBookService>();
            readerService = testHost.Services.GetService<IReaderService>();
            recordService = testHost.Services.GetService<IRecordService>();
        }

        [Theory(DisplayName = "Record Crud Test")]
        [ClassData(typeof(RecordData))]
        public async void Crud(InputBook newbook, InputReader newReader)
        {
            var bookAddMessage = await bookService.Save(newbook);
            Assert.True(bookAddMessage.Ok);

            var readerAddMessage = await readerService.Save(newReader);
            Assert.True(readerAddMessage.Ok);

            var bookInDB = await bookService.GetByDBName(newbook.DBName);
            var readerInDB = await readerService.GetByDBName(newReader.DBName);

            // Create
            var newRecord = new InputRecord(
                ReaderID: readerInDB.Id,
                BookID: bookInDB.Id
            );

            InstanceCUDMessage<int> recordAddMessage = await recordService.Save(newRecord);
            Assert.True(recordAddMessage.Ok);

            int recordID = recordAddMessage.Instances.First();

            var recordInDB = await recordService.GetByID(recordID);
            Assert.NotNull(recordInDB);

            // Update
            var newEndDate = DateTime.Now;

            var updatedRecord = new InputRecord(
                Id: recordInDB.Id,
                ReaderID: recordInDB.ReaderID,
                BookID: recordInDB.BookID,
                StartDate: recordInDB.StartDate,
                EndDate: newEndDate,
                DeleteDate: recordInDB.DeleteDate
            );

            var updateMessage = await recordService.Save(updatedRecord);
            Assert.True(updateMessage.Ok);
            Assert.Equal(1, updateMessage.NumAffected);

            var recordInDBAfterUpdate = await recordService.GetByID(recordID);
            Assert.NotNull(recordInDBAfterUpdate.EndDate);
            Assert.Equal(newEndDate, recordInDBAfterUpdate.EndDate ?? DateTime.Now, TimeSpan.FromSeconds(5));

            // Delete
            var deleteMessage = await recordService.DeleteByID(recordID);
            Assert.True(deleteMessage.Ok);
            Assert.Equal(1, deleteMessage.NumAffected);

            var recordInDBAfterDelete = await recordService.GetByID(recordID);
            Assert.Null(recordInDBAfterDelete);

        }

        public Task InitializeAsync() => checkpoint.Reset(dbContext.Connection as DbConnection);

        public Task DisposeAsync() => Task.CompletedTask;
    }

    public class RecordData : TheoryData<InputBook, InputReader>
    {
        public RecordData()
        {
            Add(
                new InputBook(
                    DBName: "book-kittenArt",
                    Title: "Kitten Art",
                    Rating: 3.8f
                ),
                new InputReader(
                    DBName: "reader-johnSmith",
                    FristName: "John",
                    LastName: "Smith",
                    IsAdult: true,
                    Phone: "321-3214827",
                    Credit: 100.333m
                )
            );
        }

    }

}
