using Domain.Enums;

namespace Domain.Errors
{
    public partial class Error
    {
        public static class Driver
        {
            public static ErrorOr.Error DuplicateCode
                => ErrorOr.Error.Conflict(
                    code: "Driver.DuplicateCode",
                    description: "Código já existe.");
            public static ErrorOr.Error NotFound
                => ErrorOr.Error.NotFound(
                    code: "Driver.NotFound",
                    description: "Componente não encontrado.");
            public static ErrorOr.Error Canceled
                => ErrorOr.Error.Custom(
                    type: ((int)CustomErrorType.CANCELED),
                    code: "Driver.Canceled",
                    description: "Operação cancelada.");
            public static ErrorOr.Error Exception(string message)
                => ErrorOr.Error.Failure(code: "Driver.Exception", description: message);
        }
    }
}
