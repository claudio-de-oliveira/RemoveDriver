using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Entity.Driver
{
    public record FileContentResponse(
        byte[] Contents,
        long Length,
        string ContentType,
        string FileName
        );
}
