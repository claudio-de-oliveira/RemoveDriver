using Application.Handlers.Driver.Common;
using Application.Interfaces.Driver;
using ErrorOr;
using MediatR;

namespace Application.Handlers.Driver.Query
{
    public class GetFolderFilesQueryHandler
        : IRequestHandler<GetFolderFilesQuery, ErrorOr<IEnumerable<FileInfoResult>>>
    {
        private readonly IDriver _driver;

        public GetFolderFilesQueryHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<IEnumerable<FileInfoResult>>> Handle(GetFolderFilesQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                await Task.CompletedTask;
                var directoryContents = _driver.GetFolderFiles(request.RelPath);

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
