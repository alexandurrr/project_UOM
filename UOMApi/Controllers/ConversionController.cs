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
        IxmlReader xmlReader = new xmlReader();
        [Route("AllDimensionUnits")]
        [HttpGet]
        public IActionResult GetUnitDimensions()
        {

            return Ok(xmlReader.ListUnitDimensions<string>());
        }

        [Route("Aliases")]
        [HttpGet]
        public IActionResult GetAliasesforUOM(string uom)
        {

            return Ok(xmlReader.FindAliasesforUOM<string>(uom));
        }

        [Route("AllQC")]
        [HttpGet]
        public IActionResult GetQC()
        {

            return Ok(xmlReader.QuantityUnits<string>());
        }

        [Route("UOMifQC")]
        [HttpPost]
        public IActionResult AllUOMforQC(string quantityclass)
        {

            return Ok(xmlReader.ListAllUOMforQC<string>(quantityclass));
        }

        [HttpPost]
        public IActionResult Post(string fromUnit, double value, string toUnit)
        {

            string post = units.conversionSequence(fromUnit, value, toUnit); 

            return Ok(post);
        }
    }
}
