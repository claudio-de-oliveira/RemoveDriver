using Application.Interfaces.Driver;
using ErrorOr;
using MediatR;

namespace Application.Handlers.Driver.Update
{
    public class MoveFileCommandHandler
        : IRequestHandler<MoveFileCommand, ErrorOr<bool>>
    {
        private readonly IDriver _driver;

        public MoveFileCommandHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<bool>> Handle(MoveFileCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                await _driver.MoveFile(request.File, request.Folder, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                return Domain.Errors.Error.Driver.Exception(ex.Message);
            }
        }
    }
}
