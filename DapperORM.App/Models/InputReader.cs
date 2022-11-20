using System;

namespace DapperORM.App.Models
{
    public record InputReader(
        int? Id = null,
        string DBName = null,
        string FristName = null,
        string LastName = null,
        bool? IsAdult = null,
        string Phone = null,
        decimal? Credit = null,
        DateTime? UpdateDate = null,
        DateTime? DeleteDate = null
    );
}