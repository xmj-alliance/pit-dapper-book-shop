using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace DapperORM.App.Database
{
    public class DBContext : IDBContext, IDisposable
    {
        private readonly IConfiguration config;

        public IDbConnection Connection { get; set; }

        public DBContext(
            IConfiguration config
        )
        {
            this.config = config;
            Connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
