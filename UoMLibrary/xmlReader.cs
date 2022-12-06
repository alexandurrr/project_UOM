using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UoMLibrary
{
    public class xmlReader : IxmlReader
    {
        public List<T> ListUnitDimensions<T>()
        {
            List<T> unitDimensions = new List<T>();
            unitDimensions.Add((T)Convert.ChangeType("Symbol   Definition/Meaning  SI Unit (if applicable)", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("A  angle   radian", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("D  temperature difference  kelvin", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("I  electrical current  ampere", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("J  luminous intensity  candela", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("K  thermodynamic temperature", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("L  length  meter", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("M  mass    kilogram", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("N  amount of substance  mole", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("S  solid angle  steradian", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("T  time  second", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("1  the number one  NA", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("2  squared (e.g., M2)  NA", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("3  cubed  NA", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("4  4th power (5=5th, etc.)  NA", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("/  division  NA", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("none  Special notation indicating  NA", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("        unit for which a normal", typeof(T)));
            unitDimensions.Add((T)Convert.ChangeType("        dimensionless analysis is possible.", typeof(T)));
            return unitDimensions;
        }
        public List<T> FindAliasesforUOM<T>(string uom)
        {
            List<T> aliases = new List<T>();
            var tupleList = new List<(int, string)> { };
            if (uom == "meter" || uom == "metre")
            {
                aliases.Add((T)Convert.ChangeType("m", typeof(T)));
                aliases.Add((T)Convert.ChangeType("running distance in the desert", typeof(T)));
                aliases.Add((T)Convert.ChangeType("ferret meters", typeof(T)));

            }
            else
            {
                aliases.Add((T)Convert.ChangeType("none", typeof(T)));
            }

            return aliases;
        }
        public List<T> QuantityUnits<T>()
        {

            List<T> quantityClasses = new List<T>();
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if (xmlReader.Name.ToString() == "QuantityType")
                    {
                        string quantityType = xmlReader.ReadElementContentAsString();

                        // Ensure we have a proper string to search for.
                        if (!string.IsNullOrEmpty(quantityType))
                        {
                            if (!quantityClasses.Contains((T)Convert.ChangeType(quantityType, typeof(T))))
                                quantityClasses.Add((T)Convert.ChangeType(quantityType, typeof(T)));
                        }

                    }
                }

            }
            quantityClasses.RemoveAt(0);
            return quantityClasses;
        }
        public List<T> ListAllUOMforQC<T>(string selectedText)
        {
            List<T> unitOfMeasures = new List<T>();
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if ("Name" == xmlReader.Name.ToString())
                    {
                        string name = xmlReader.ReadString();
                        while (xmlReader.ReadToNextSibling("QuantityType") == true)
                        {
                            string quantityType = xmlReader.ReadElementContentAsString();

                            if (!string.IsNullOrEmpty(name) && quantityType == selectedText)
                            {
                                if (!unitOfMeasures.Contains((T)Convert.ChangeType(name, typeof(T))))
                                    unitOfMeasures.Add((T)Convert.ChangeType(name, typeof(T)));

                            }

                        }
                    }
                }

            }
            return unitOfMeasures;
        }
    }
}
