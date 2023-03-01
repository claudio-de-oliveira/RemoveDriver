using Application.Handlers.Driver.Common;

using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Create
{
    public record CreateFolderCommand(
        string RelPath
        ) : IRequest<ErrorOr<string>>;
}
