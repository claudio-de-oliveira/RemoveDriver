using FluentValidation;

namespace Application.Handlers.Driver.Delete
{
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {

        }
    }
}
