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
        [Route("AllQC")]
        [HttpGet]
        public IActionResult Get()
        {

            return Ok(units.QuantityUnits<string>());
        }

        [Route("UOMifQC")]
        [HttpPost]
        public IActionResult AllUOMforQC(string quantityclass)
        {

            return Ok(units.ListAllUOMforQC<string>(quantityclass));
        }

        [HttpPost]
        public IActionResult Post(string fromUnit, double value, string toUnit)
        {

            string post = units.conversionSequence(fromUnit, value, toUnit); 

            return Ok(post);
        }
    }
}
