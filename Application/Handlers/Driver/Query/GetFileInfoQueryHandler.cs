using Application.Handlers.Driver.Common;
using Application.Interfaces.Driver;

using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Query
{
    public class GetFileInfoQueryHandler
        : IRequestHandler<GetFileInfoQuery, ErrorOr<FileInfoResult>>
    {
        private readonly IDriver _driver;

        public GetFileInfoQueryHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<FileInfoResult>> Handle(GetFileInfoQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                await Task.CompletedTask;
                var info = _driver.GetFileInfo(request.RelPath);
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
