namespace DapperORM.App.Models
{
    public record CUDMessage(
        bool Ok,
        long NumAffected,
        string Message
    );
}