using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoMLibrary
{
    public interface IUnits
    {
        List<T> QuantityUnits<T>();
        List<T> ListAllUOM<T>();
        List<T> ListAllUOMforQC<T>(string selectedText);
        List<T> ListAllSameBaseUnit<T> (string baseunit, string textName, string addBase);
        string findTypeOfConversion(string baseunitName);
        string findAnnotation(string annotation);
        string checkIfOther(string baseunitName);
        string getBaseUnit(string baseunitName);
        string getBaseUnitName(string baseunitName, string annotation);
        string convertToBase(string baseunitName, double value, string conversionType);
        string convert_to_final(string baseunitName, double value, string conversionType, string annotation, int selectedIndex);
        string conversionSequence(string fromUnit, double value, string toUnit);

    }
}
