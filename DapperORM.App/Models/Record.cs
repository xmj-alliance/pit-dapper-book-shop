using System;

namespace DapperORM.App.Models
{
    public record Record(
        int Id,
        int ReaderID,
        int BookID,
        DateTime StartDate,
        DateTime? EndDate,
        DateTime UpdateDate,
        DateTime? DeleteDate
    )
    {
        // Hack for Dapper 2.0.90 (which has no support for C# records)
        public Record() : this(default, default, default, default, default, default, default) { }
    };
}