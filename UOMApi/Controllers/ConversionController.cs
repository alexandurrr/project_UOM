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
        bool isbaseUnit = false;
        string nameBase;
        List<string> allUnits = new List<string>();  



        [HttpGet]
        public IActionResult Get(string fromUnit, double value, string toUnit)
        {
            string test = fromUnit+ ":" + value+ ":" + toUnit;
            return Ok(test);
        }
        [HttpPost]
        public IActionResult Post(string fromUnit, double value, string toUnit)
        {
            List<string> quantityClasses = new List<string>();
            isbaseUnit = false;
            nameBase = "";
            string postFinal;
            postFinal = string.Empty;


            IUnits units = new Units();
            if (allUnits.Count == 0)
            {
                allUnits.AddRange(units.ListAllUOM<string>());
            }
            if (!allUnits.Contains(fromUnit))
            {
                return BadRequest("fromUnit not found or is not convertable!");
            }

            string baseunit = units.getBaseUnit(fromUnit);
            if (baseunit.Contains("Base unit:"))
            {
                string result = String.Join(" ", baseunit.Split(' ').Skip(2));
                string annotation = units.findAnnotation(result);
                units.ListAllSameBaseUnit<string>(annotation, result, "Base unit: none").ForEach(i => quantityClasses.Add(i));
                isbaseUnit = true;

            }
            else
            {
                string baseUnitName = units.getBaseUnitName(fromUnit, baseunit);
                nameBase = baseUnitName;
                units.ListAllSameBaseUnit<string>(baseunit, fromUnit, baseUnitName).ForEach(i => quantityClasses.Add(i));
            }

            int selectedIndex = 0;
            if (nameBase == toUnit) { selectedIndex = 1; }
            if (!quantityClasses.Contains(toUnit))
            {
                return BadRequest("toUnit not found or is not convertable!");
            }


            double num = value;
                if (isbaseUnit && !toUnit.Contains("Base unit: none"))
                {
                    string annotation2 = units.findAnnotation(toUnit);
                    string toConversionName2 = units.findTypeOfConversion(toUnit);
                    string resultfinal = units.convert_to_final(toUnit, num, toConversionName2, annotation2, selectedIndex);
                    postFinal = resultfinal;
                    isbaseUnit = true;
                }
                else if (!toUnit.Contains("Base unit: none"))
                {
                    string fromConversionName = units.findTypeOfConversion(fromUnit);
                    string toConversionName = units.findTypeOfConversion(toUnit);
                    string from_result = units.convertToBase(fromUnit, num, fromConversionName);
                    double newval;
                    newval = double.Parse(from_result, CultureInfo.InvariantCulture);
                    string annotation = units.findAnnotation(toUnit);

                    string resultfinal = units.convert_to_final(toUnit, newval, toConversionName, annotation, selectedIndex);
                    postFinal = resultfinal;
                    isbaseUnit = false;
                
            }
            return Ok(postFinal);
        }
    }
}
