using Application.Interfaces.Driver;
using ErrorOr;
using MediatR;

namespace Application.Handlers.Driver.Update
{
    public class RenameFolderCommandHandler
        : IRequestHandler<RenameFolderCommand, ErrorOr<bool>>
    {
        private readonly IDriver _driver;

        public RenameFolderCommandHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<bool>> Handle(RenameFolderCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                await Task.CompletedTask;
                _driver.RenameFolder(request.OldName, request.NewName);
                return true;
            }
            catch (Exception ex)
            {
                return Domain.Errors.Error.Driver.Exception(ex.Message);
            }
        }
    }
}
