using FluentValidation;

namespace Application.Handlers.Driver.Create
{
    public class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
    {
        public CreateFolderCommandValidator()
        { 
        }
    }
}
