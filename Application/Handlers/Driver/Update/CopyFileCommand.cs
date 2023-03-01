using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Update
{
    public record CopyFileCommand(
        // string Root,
        string Source,
        string Target
        ) : IRequest<ErrorOr<bool>>;
}
