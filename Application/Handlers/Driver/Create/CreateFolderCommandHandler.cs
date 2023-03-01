using Application.Interfaces.Driver;

using ErrorOr;

using MediatR;

namespace Application.Handlers.Driver.Create
{
    public class CreateFolderCommandHandler
        : IRequestHandler<CreateFolderCommand, ErrorOr<string>>
    {
        private readonly IDriver _driver;

        public CreateFolderCommandHandler(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<ErrorOr<string>> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Domain.Errors.Error.Driver.Canceled;

            try
            {
                await Task.CompletedTask;
                return _driver.CreateFolder(request.RelPath);
            }
            catch (Exception ex)
            {
                return Domain.Errors.Error.Driver.Exception(ex.Message);
            }
        }
    }
}
