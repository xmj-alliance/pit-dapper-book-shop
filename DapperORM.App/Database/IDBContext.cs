using System.Data;

namespace DapperORM.App.Database
{
    public interface IDBContext
    {
        IDbConnection Connection { get; }
    }
}