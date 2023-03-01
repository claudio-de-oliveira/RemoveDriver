using Application.Handlers.Driver.Common;

using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Query
{
    public record DownloadFileQuery(
        // string Root,
        string FileName
        ) : IRequest<ErrorOr<CustomFileContentResult>>;
}
