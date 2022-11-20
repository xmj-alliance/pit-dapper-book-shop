using System.Collections.Generic;

namespace DapperORM.App.Models
{
    public record InstanceCUDMessage<T>(
        bool Ok,
        long NumAffected,
        string Message,
        IEnumerable<T> Instances
    );
}