using Application.Handlers.Driver.Common;
using Application.Interfaces.Driver;
using ErrorOr;
using MediatR;

namespace Application.Handlers.Driver.Query
{
    public class GetFilesQueryHandler
        : IRequestHandler<GetRootFilesQuery, ErrorOr<IEnumerable<FileInfoResult>>>
    {
        private readonly IDriver _driver;

        public GetFilesQueryHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<IEnumerable<FileInfoResult>>> Handle(GetRootFilesQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                await Task.CompletedTask;
                var directoryContents = _driver.GetRootFiles();

                return directoryContents.Select(
                    fileInfo => new FileInfoResult(
                        fileInfo.Exists,
                        fileInfo.Length,
                        fileInfo.PhysicalPath,
                        fileInfo.Name,
                        fileInfo.LastModified,
                        fileInfo.IsDirectory
                        )).ToList();
            }
            catch (Exception ex)
            {
                return Domain.Errors.Error.Driver.Exception(ex.Message);
            }
        }
    }
}
