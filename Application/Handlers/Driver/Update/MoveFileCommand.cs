using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Update
{
    public record MoveFileCommand(
        // string Root,
        string File,
        string Folder
        ) : IRequest<ErrorOr<bool>>;
}
