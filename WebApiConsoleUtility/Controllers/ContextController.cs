using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParserLibrary;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;
using System.Threading.Tasks;

namespace WebApiConsoleUtility.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContextController : ControllerBase
    {
        [HttpGet("GetContext")]
        [SwaggerOperation(
Summary = "Возвращает контекст трассы",
Description = "При вызове возвращает 200 (OK)"
, Tags = new[] { "Context" }
)]
        [SwaggerResponse(200, "Жив- здоров")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //        public string Post([FromBody, SwaggerParameter("Дата время формирования операции", Required = true)] DateTime timeCreate, [FromQuery, SwaggerParameter("Номер телефона клиента MobiCash", Required = true)] string PhoneMobicashClient)
        public async Task<string> GetHealthCheck(string fileName)
        {
            fileName = Path.Combine(Pipeline.traceSaveDirectoryStat, fileName.Replace("_","."));
            if(System.IO.File.Exists(fileName))
            using(StreamReader sw = new StreamReader(fileName)) 
            {
                    return sw.ReadToEnd();
            }

            return "";
        }

    }
}
