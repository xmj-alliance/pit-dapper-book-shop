using System;

namespace DapperORM.App.Models
{
    public record InputRecord(
        int? Id = null,
        int? ReaderID = null,
        int? BookID = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        DateTime? UpdateDate = null,
        DateTime? DeleteDate = null
    );
}