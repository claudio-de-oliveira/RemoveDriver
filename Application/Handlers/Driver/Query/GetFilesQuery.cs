using Application.Handlers.Driver.Common;

using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Query
{
    public record GetRootFilesQuery(
        //
        ) : IRequest<ErrorOr<IEnumerable<FileInfoResult>>>;
}
