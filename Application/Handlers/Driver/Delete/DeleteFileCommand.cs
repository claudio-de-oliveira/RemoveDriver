using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Delete
{
    public record DeleteFileCommand(
        // string Root,
        string FileName
        ) : IRequest<ErrorOr<bool>>;
}
