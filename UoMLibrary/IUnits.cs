using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoMLibrary
{
    public interface IUnits
    {
        List<T> ListAllUOM<T>();
        List<T> ListAllSameBaseUnit<T> (string baseunit, string textName, string addBase);
        string findTypeOfConversion(string baseunitName);
        string findAnnotation(string annotation);
        string checkIfOther(string baseunitName);
        string getBaseUnit(string baseunitName);
        string getBaseUnitName(string baseunitName, string annotation);
        string conversionSequence(string fromUnit, double value, string toUnit);
        string conversionSequenceGUI(string fromUnit, string value, string toUnit, bool isBaseUnit, int selectedIndex);


    }
}
