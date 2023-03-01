using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Update
{
    public record MoveFolderCommand(
        // string Root,
        string Source,
        string Target
        ) : IRequest<ErrorOr<bool>>;
}
