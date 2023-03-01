using Application.Handlers.Driver.Common;
using Application.Interfaces.Driver;
using ErrorOr;
using MediatR;

using Shared;

namespace Application.Handlers.Driver.Query
{
    public class DownloadFileQueryHandler
        : IRequestHandler<DownloadFileQuery, ErrorOr<CustomFileContentResult>>
    {
        private readonly IDriver _driver;

        public DownloadFileQueryHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<CustomFileContentResult>> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            await Task.CompletedTask;

            try
            {
                Console.WriteLine("Estou dentro do DownloadFileQueryHandler 1");
                byte[] buffer = await _driver.DownloadFile(request.FileName, cancellationToken);

                Console.WriteLine("Estou dentro do DownloadFileQueryHandler 2");
                var extension = Path.GetExtension(request.FileName);
                var contentType = MimeTypes.ContentType(extension);

                return new CustomFileContentResult(buffer, contentType, Path.GetFileName(request.FileName));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Estou dentro do DownloadFileQueryHandler 3");
                return Domain.Errors.Error.Driver.Exception(ex.Message);
            }
        }
    }
}
