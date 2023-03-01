using Application.Interfaces.Driver;
using ErrorOr;
using MediatR;

namespace Application.Handlers.Driver.Delete
{
    public class DeleteFolderCommandHandler
        : IRequestHandler<DeleteFolderCommand, ErrorOr<bool>>
    {
        private readonly IDriver _driver;

        public DeleteFolderCommandHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<bool>> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                await Task.CompletedTask;
                _driver.DeleteFolder(request.FolderName);
                return true;
            }
            catch (Exception ex)
            {
                return Domain.Errors.Error.Driver.Exception(ex.Message);
            }
        }
    }
}
