using Application.Handlers.Driver.Common;

using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Query
{
    public record GetFolderFilesQuery(
        // string Root,
        string RelPath
        ) : IRequest<ErrorOr<IEnumerable<FileInfoResult>>>;
}
