using DapperORM.App.Database;
using DapperORM.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DapperORM.UnitTest
{
    public class ServiceFixture
    {
        public IHost TestHost { get; set; }

        public ServiceFixture()
        {

            TestHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // configure db
                    services.AddTransient<IDBContext, DBContext>();

                    // add services
                    services.AddSingleton<IBookService, BookService>();
                    services.AddSingleton<IReaderService, ReaderService>();
                    services.AddSingleton<IRecordService, RecordService>();
                })
                .Build();

        }

    }
}
