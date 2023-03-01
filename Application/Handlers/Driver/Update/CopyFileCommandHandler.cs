using Application.Interfaces.Driver;
using ErrorOr;
using MediatR;

namespace Application.Handlers.Driver.Update
{
    public class CopyFileCommandHandler
        : IRequestHandler<CopyFileCommand, ErrorOr<bool>>
    {
        private readonly IDriver _driver;

        public CopyFileCommandHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<bool>> Handle(CopyFileCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                await _driver.CopyFile(request.Source, request.Target, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                return Domain.Errors.Error.Driver.Exception(ex.Message);
            }
        }
    }
}
