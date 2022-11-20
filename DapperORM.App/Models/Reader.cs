using System;

namespace DapperORM.App.Models
{
    public record Reader(
        int Id,
        string DBName,
        string FristName,
        string LastName,
        bool IsAdult,
        string Phone,
        decimal Credit,
        DateTime UpdateDate,
        DateTime? DeleteDate
    )
    {
        // Hack for Dapper 2.0.90 (which has no support for C# records)
        public Reader() : this(default, default, default, default, default, default, default, default, default) { }
    };
}