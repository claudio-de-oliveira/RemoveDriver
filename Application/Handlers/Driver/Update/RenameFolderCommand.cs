using ErrorOr;
using MediatR;

namespace Application.Handlers.Driver.Update
{
    public record RenameFolderCommand(
        // string Root,
        string OldName,
        string NewName
        ) : IRequest<ErrorOr<bool>>;
}
