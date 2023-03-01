namespace Application.Handlers.Driver.Common
{
    public record FileInfoResult(
        bool Exists,
        long Length,
        string? PhysicalPath,
        string Name,
        DateTimeOffset ModifiedOnUtc,
        bool IsDirectory
        );
}
