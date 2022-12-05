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
        public string ListUnitDimensions()
        {
            string unitDimensions = String.Format("Symbol   Definition/Meaning  SI Unit (if applicable)\n" +
                "A  angle   radian\n" +
                "D  temperature difference  kelvin\n" +
                "I  electrical current  ampere\n" +
                "J  luminous intensity  candela\n" +
                "K  thermodynamic temperature\n" +
                "L  length  meter\n" +
                "M  mass    kilogram\n" +
                "N  amount of substance  mole\n" +
                "S  solid angle  steradian\n" +
                "T  time  second\n" +
                "1  the number one  NA\n" +
                "2  squared (e.g., M2)  NA\n" +
                "3  cubed  NA\n" +
                "4  4th power (5=5th, etc.)  NA\n" +
                "/  division  NA\n" +
                "none  Special notation indicating unit  NA\n" +
                "      for which a normal\n" +
                "      dimensionless analysis is possible.");
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
