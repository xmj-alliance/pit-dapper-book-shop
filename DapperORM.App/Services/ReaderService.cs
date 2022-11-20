using DapperORM.App.Database;
using DapperORM.App.Models;

namespace DapperORM.App.Services
{
    public class ReaderService: DataAccessService<Reader, InputReader>, IReaderService
    {
        private readonly IDBContext dbContext;

        public ReaderService(
            IDBContext dbContext
        ): base(
            dbContext,
            "reader"
        )
        {
            this.dbContext = dbContext;
        }

    }
}