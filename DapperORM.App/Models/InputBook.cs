using System;

namespace DapperORM.App.Models
{
    public record InputBook(
        int? Id = null,
        string DBName = null,
        string Title = null,
        float? Rating = null,
        DateTime? UpdateDate = null,
        DateTime? DeleteDate = null
    );
}