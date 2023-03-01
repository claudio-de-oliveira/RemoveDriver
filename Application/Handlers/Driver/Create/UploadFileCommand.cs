using Application.Handlers.Driver.Common;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace Application.Handlers.Driver.Create
{
    public record UploadFileCommand(
        // string Root,
        IFormFile File
        ) : IRequest<ErrorOr<FileInfoResult>>;
}
