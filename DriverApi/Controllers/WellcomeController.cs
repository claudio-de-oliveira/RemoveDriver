using Microsoft.AspNetCore.Mvc;

namespace DriverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WellcomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Wellcome to Driver API";
        }
    }
}
