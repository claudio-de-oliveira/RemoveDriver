namespace Application.Handlers.Driver.Common
{
    public record CustomFileContentResult(
        byte[] Buffer,
        string ContentType,
        string FileName
        );
}
