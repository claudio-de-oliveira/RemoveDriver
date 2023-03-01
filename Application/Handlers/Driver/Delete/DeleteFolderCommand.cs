using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Delete
{
    public record DeleteFolderCommand(
        // string Root,
        string FolderName
        ) : IRequest<ErrorOr<bool>>;
}
