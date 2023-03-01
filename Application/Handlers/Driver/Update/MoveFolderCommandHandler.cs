using Application.Interfaces.Driver;
using ErrorOr;
using MediatR;

namespace Application.Handlers.Driver.Update
{
    public class MoveFolderCommandHandler
        : IRequestHandler<MoveFolderCommand, ErrorOr<bool>>
    {
        private readonly IDriver _driver;

        public MoveFolderCommandHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<bool>> Handle(MoveFolderCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                await Task.CompletedTask;
                _driver.MoveFolder(request.Source, request.Target);
                return true;
            }
            catch (Exception ex)
            {
                return Domain.Errors.Error.Driver.Exception(ex.Message);
            }
        }
    }
}
