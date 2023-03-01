namespace Contracts.Entity.Driver
{
    public record FileInfoResponse(
        bool Exists,
        long Length,
        string? PhysicalPath,
        string Name,
        DateTimeOffset ModifiedOnUtc,
        bool IsDirectory
        );
}
