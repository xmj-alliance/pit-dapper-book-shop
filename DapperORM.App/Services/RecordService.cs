using DapperORM.App.Database;
using DapperORM.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperORM.App.Services
{
    public class RecordService: DataAccessService<Record, InputRecord>, IRecordService
    {
        private readonly IDBContext dbContext;

        public RecordService(
            IDBContext dbContext
        ): base(
            dbContext,
            "record"
        )
        {
            this.dbContext = dbContext;
        }

        [Obsolete("Association class cannot query by dbname.", true)]
        public new Record GetByDBName(string dbname)
        {
            throw new NotSupportedException("Association class cannot query by dbname.");
        }

        [Obsolete("Association class cannot query by dbname.", true)]
        public new IEnumerable<Record> GetByDBName(IEnumerable<string> dbnames)
        {
            throw new NotSupportedException("Association class cannot query by dbname.");
        }

    }
}