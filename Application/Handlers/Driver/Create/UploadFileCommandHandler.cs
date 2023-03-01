using Application.Handlers.Driver.Common;
using Application.Interfaces.Driver;

using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Create
{
    public class UploadFileCommandHandler
        : IRequestHandler<UploadFileCommand, ErrorOr<FileInfoResult>>
    {
        private readonly IDriver _driver;

        public UploadFileCommandHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<FileInfoResult>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                var info = await _driver.UploadFile(request.File, cancellationToken);
                if (!info.Exists)
                    return Domain.Errors.Error.Driver.NotFound;

                return new FileInfoResult(
                        info.Exists,
                        info.Length,
                        info.PhysicalPath,
                        info.Name,
                        info.LastModified,
                        info.IsDirectory
                        );
            }
            catch (Exception ex)
            {
                return Domain.Errors.Error.Driver.Exception(ex.Message);
            }
        }
    }
}
