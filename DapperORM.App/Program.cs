using DapperORM.App.Database;
using DapperORM.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DapperORM.App
{
    class Program
    {
        static void Main(string[] args)
        {

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // configure db
                    services.AddTransient<IDBContext, DBContext>();

                    // services
                    services.AddSingleton<IBookService, BookService>();
                })
                .Build();

            //var bookService = ActivatorUtilities.CreateInstance<BookService>(host.Services);
            //var db = ActivatorUtilities.CreateInstance<DBContext>(host.Services);

        }

    }
}
