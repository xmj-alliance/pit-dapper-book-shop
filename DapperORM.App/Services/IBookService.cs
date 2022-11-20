using DapperORM.App.Models;

namespace DapperORM.App.Services
{
    public interface IBookService : IDataAccessService<Book, InputBook>
    {
    }
}
