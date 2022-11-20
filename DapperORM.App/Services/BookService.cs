using DapperORM.App.Database;
using DapperORM.App.Models;

namespace DapperORM.App.Services
{
    public class BookService: DataAccessService<Book, InputBook>, IBookService
    {
        private readonly IDBContext dbContext;

        public BookService(
            IDBContext dbContext
        ): base(
            dbContext,
            "book"
        )
        {
            this.dbContext = dbContext;
        }

    }
}