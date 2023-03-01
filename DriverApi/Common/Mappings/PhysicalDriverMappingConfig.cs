using Application.Handlers.Driver.Common;
using Contracts.Entity.Driver;
using Mapster;

namespace DriverApi.Common.Mappings
{
    public class DriverMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CustomFileContentResult, FileContentResponse>()
                .MapWith(s => MapFileContentResult(s));
            config.NewConfig<FileInfoResult, FileInfoResponse>();
        }

        private static FileContentResponse MapFileContentResult(CustomFileContentResult source)
            => new(source.Buffer, source.Buffer.LongLength, source.ContentType, source.FileName);
    }
}
