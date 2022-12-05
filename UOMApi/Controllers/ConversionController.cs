using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using UoMLibrary;

namespace UOMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        IUnits units = new Units();
        [HttpGet]
        public IActionResult Get(string fromUnit, double value, string toUnit)
        {
            string test = fromUnit+ ":" + value+ ":" + toUnit;
            return Ok(test);
        }
        [HttpPost]
        public IActionResult Post(string fromUnit, double value, string toUnit)
        {

            string post = units.conversionSequence(fromUnit, value, toUnit); 

            return Ok(post);
        }
    }
}
