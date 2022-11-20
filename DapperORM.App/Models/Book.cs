using System;

namespace DapperORM.App.Models
{
    public record Book(
        int Id,
        string DBName,
        string Title,
        float Rating,
        DateTime UpdateDate,
        DateTime? DeleteDate
    )
    {
        // Hack for Dapper 2.0.90 (which has no support for C# records)
        public Book() : this(default, default, default, default, default, default) { }
    };
}