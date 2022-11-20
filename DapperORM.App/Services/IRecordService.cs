using DapperORM.App.Models;
using System;
using System.Collections.Generic;

namespace DapperORM.App.Services
{
    public interface IRecordService : IDataAccessService<Record, InputRecord>
    {
        [Obsolete("Association class cannot query by dbname.", true)]
        new Record GetByDBName(string dbname);
        [Obsolete("Association class cannot query by dbname.", true)]
        new IEnumerable<Record> GetByDBName(IEnumerable<string> dbnames);
    }
}
