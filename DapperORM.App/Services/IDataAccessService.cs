using DapperORM.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperORM.App.Services
{
    public interface IDataAccessService<T, TInput>
    {
        Task<InstanceCUDMessage<int>> Save(TInput newItem, string storedProcedureName = null, string spInputParamName = null, string sqlInputTypeName = null);
        Task<InstanceCUDMessage<int>> Save(IEnumerable<TInput> newItems, string storedProcedureName = null, string spInputParamName = null, string sqlInputTypeName = null);
        Task<T> GetByID(int id);
        Task<IEnumerable<T>> GetByID(IEnumerable<int> ids);
        Task<T> GetByDBName(string dbname);
        Task<IEnumerable<T>> GetByDBName(IEnumerable<string> dbnames);
        Task<CUDMessage> DeleteByID(int id);
        Task<CUDMessage> DeleteByID(IEnumerable<int> ids);
    }
}
