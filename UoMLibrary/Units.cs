using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace UoMLibrary
{
    public class Units : IUnits
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
                "none  Special notation indicating unit  NA\n"+
                "      for which a normal\n"+
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
        public List<T> ListAllUOM<T>()
        {
            List<T> names = new List<T>();
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if (xmlReader.Name.ToString() == "Name")
                    {
                        string name = xmlReader.ReadElementContentAsString();
                        // Ensure we have a proper string to search for.
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (!names.Contains((T)Convert.ChangeType(name, typeof(T))))
                                names.Add((T)Convert.ChangeType(name, typeof(T)));
                        }
                    }
                }

            }
            names.RemoveAt(0);
            return names;
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

        public List<T> ListAllSameBaseUnit<T>(string baseunit, string textName, string addBase)
        {

            List<T> baseUnits = new List<T>();
            baseUnits.Add((T)Convert.ChangeType(addBase, typeof(T)));
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if ("Name" == xmlReader.Name.ToString())
                    {
                        string name = xmlReader.ReadString();

                        if (name != textName)
                        {
                            if (xmlReader.ReadToNextSibling("ConversionToBaseUnit"))
                            {
                                string tobaseUnit = xmlReader.GetAttribute("baseUnit");
                                if (tobaseUnit == baseunit && !name.Contains("POSC Units of Measure Dictionary"))
                                {
                                    baseUnits.Add((T)Convert.ChangeType(name, typeof(T)));

                                }

                            }
                        }
                    }
                }

            }
            return baseUnits;
        }

        public string findTypeOfConversion(string baseunitName)
        {
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if ("Name" == xmlReader.Name.ToString())
                    {
                        string name = xmlReader.ReadString();
                        if (name == baseunitName)
                        {
                            if (xmlReader.ReadToNextSibling("ConversionToBaseUnit"))
                            {
                                if (xmlReader.ReadToDescendant("Formula"))
                                {
                                    baseunitName = "Formula";
                                }
                                else
                                {
                                    baseunitName = checkIfOther(baseunitName);
                                }

                            }
                        }
                    }
                }

            }
            return baseunitName;
        }

        public string findAnnotation(string annotation)
        {
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"); 
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if ("Name" == xmlReader.Name.ToString())
                    {
                        string name = xmlReader.ReadString();
                        if (annotation.Contains("Base unit: "))
                        {
                            string basename = String.Join(" ", annotation.Split(' ').Skip(2));
                            if (name == basename)
                            {
                                if (xmlReader.ReadToNextSibling("CatalogSymbol"))
                                {

                                    annotation = xmlReader.ReadElementContentAsString();

                                }
                            }
                        }
                        else if (name == annotation)
                        {
                            if (xmlReader.ReadToNextSibling("CatalogSymbol"))
                            {

                                annotation = xmlReader.ReadElementContentAsString();

                            }
                        }
                    }
                }

            }
            return annotation;
        }

        public string checkIfOther(string baseunitName)
        {
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if ("Name" == xmlReader.Name.ToString())
                    {
                        string name = xmlReader.ReadString();
                        if (name == baseunitName)
                        {
                            if (xmlReader.ReadToNextSibling("ConversionToBaseUnit"))
                            {
                                if (xmlReader.ReadToDescendant("Factor"))
                                {
                                    baseunitName = "Factor";
                                }
                                else
                                {
                                    baseunitName = "Fraction";
                                }

                            }
                        }
                    }
                }

            }
            return baseunitName;
        }

        public string getBaseUnit(string baseunitName)
        {
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if ("Name" == xmlReader.Name.ToString())
                    {
                        string name = xmlReader.ReadString();
                        if (name == baseunitName)
                        {
                            if (xmlReader.ReadToNextSibling("ConversionToBaseUnit"))
                            {
                                string baseUnit = xmlReader.GetAttribute("baseUnit");
                                baseunitName = baseUnit;
                            }
                            else
                            {
                                baseunitName = String.Format("Base unit: {0}", name);

                            }
                        }
                    }
                }

            }
            return baseunitName;
        }

        public string getBaseUnitName(string baseunitName, string annotation)
        {
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if ("Name" == xmlReader.Name.ToString())
                    {
                        string name = xmlReader.ReadString();
                        if (xmlReader.ReadToNextSibling("CatalogSymbol"))
                        {
                            string baseAnnotation = xmlReader.ReadString();

                            if (baseAnnotation == annotation && xmlReader.ReadToNextSibling("BaseUnit"))
                            {
                                string baseUnit = name;
                                baseunitName = baseUnit;
                                return baseunitName;
                            }

                        }


                    }
                }

            }
            return "nobase";
        }

        public string convertToBase(string baseunitName, double value, string conversionType)
        {
            XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if ("Name" == xmlReader.Name.ToString())
                    {
                        string name = xmlReader.ReadString();
                        if (name == baseunitName)
                        {
                            xmlReader.ReadToNextSibling("ConversionToBaseUnit");

                            if (conversionType == "Factor")
                            {
                                xmlReader.ReadToDescendant("Factor");
                                string factor = xmlReader.ReadString();
                                double d = double.Parse(factor, CultureInfo.InvariantCulture);
                                var result = (0 + (double)d * value) / (1 + 0 * value);
                                baseunitName = result.ToString();
                                return baseunitName;
                            }

                            else if (conversionType == "Fraction")
                            {
                                xmlReader.ReadToDescendant("Fraction");
                                xmlReader.ReadToDescendant("Numerator");
                                string s_numerator = xmlReader.ReadString();
                                double numerator = double.Parse(s_numerator, CultureInfo.InvariantCulture);
                                xmlReader.ReadToNextSibling("Denominator");
                                string s_denominator = xmlReader.ReadString();
                                double denominator = double.Parse(s_denominator, CultureInfo.InvariantCulture);
                                var result = (0 + (double)denominator) / (0 + (double)numerator) * value;
                                baseunitName = result.ToString();
                                return baseunitName;
                            }

                            else
                            {
                                xmlReader.ReadToDescendant("Formula");
                                xmlReader.ReadToDescendant("A");
                                string s_a = xmlReader.ReadString();
                                double a = double.Parse(s_a, CultureInfo.InvariantCulture);

                                xmlReader.ReadToNextSibling("B");
                                string s_b = xmlReader.ReadString();
                                double b = double.Parse(s_b, CultureInfo.InvariantCulture);

                                xmlReader.ReadToNextSibling("C");
                                string s_c = xmlReader.ReadString();
                                double c = double.Parse(s_c, CultureInfo.InvariantCulture);

                                xmlReader.ReadToNextSibling("D");
                                string s_d = xmlReader.ReadString();
                                double d = double.Parse(s_d, CultureInfo.InvariantCulture);

                                var result = (a + b * value) / (c + d * value);
                                baseunitName = result.ToString();
                                return baseunitName;
                            }
                        }

                    }
                }
            }
            return null;
        }

        public string convert_to_final(string baseunitName, double value, string conversionType, string annotation, int selectedIndex)
        {
            string textBox3;
            if (selectedIndex == 1)
            {
                textBox3 = String.Format("Conversion: {0}, \n{1} {2}", baseunitName, value, annotation);
                baseunitName = textBox3;
                return baseunitName;
            }
            else
            {
                XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml");
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement())
                    {
                        if ("Name" == xmlReader.Name.ToString())
                        {
                            string name = xmlReader.ReadString();
                            if (name == baseunitName)
                            {
                                xmlReader.ReadToNextSibling("ConversionToBaseUnit");
                                if (conversionType == "Factor" && xmlReader.ReadToDescendant("Factor"))
                                {
                                    string factor = xmlReader.ReadString();
                                    double d = double.Parse(factor, CultureInfo.InvariantCulture);
                                    var result = (0 - value) / (0 - (double)d);
                                    textBox3 = String.Format("Conversion: {0}, is \n{1} {2}", name, result, annotation);
                                    baseunitName = textBox3;
                                    return baseunitName;
                                }
                                else if (conversionType == "Fraction")
                                {
                                    xmlReader.ReadToDescendant("Fraction");
                                    xmlReader.ReadToDescendant("Numerator");
                                    string s_numerator = xmlReader.ReadString();
                                    double numerator = double.Parse(s_numerator, CultureInfo.InvariantCulture);
                                    xmlReader.ReadToNextSibling("Denominator");
                                    string s_denominator = xmlReader.ReadString();
                                    double denominator = double.Parse(s_denominator, CultureInfo.InvariantCulture);
                                    var result = (0 - (double)numerator) / (0 - (double)denominator) * value;
                                    textBox3 = String.Format("Conversion: {0}, is \n{1} {2}", name, result, annotation);
                                    baseunitName = textBox3;
                                    return baseunitName;
                                }
                                else
                                {
                                    xmlReader.ReadToDescendant("Formula");
                                    xmlReader.ReadToDescendant("A");
                                    string s_a = xmlReader.ReadString();
                                    double a = double.Parse(s_a, CultureInfo.InvariantCulture);

                                    xmlReader.ReadToNextSibling("B");
                                    string s_b = xmlReader.ReadString();
                                    double b = double.Parse(s_b, CultureInfo.InvariantCulture);

                                    xmlReader.ReadToNextSibling("C");
                                    string s_c = xmlReader.ReadString();
                                    double c = double.Parse(s_c, CultureInfo.InvariantCulture);

                                    xmlReader.ReadToNextSibling("D");
                                    string s_d = xmlReader.ReadString();
                                    double d = double.Parse(s_d, CultureInfo.InvariantCulture);

                                    var result = (a - c * value) / (d * value - b);
                                    baseunitName = result.ToString();
                                    textBox3 = String.Format("Conversion: {0}, is \n{1} {2}", name, result, annotation);
                                    baseunitName = textBox3;
                                    return baseunitName;
                                }

                            }
                        }
                    }
                }
            }
            return "";
        }

        public string conversionSequence(string fromUnit, double value, string toUnit)
        {
            IUnits units = new Units();
            bool isbaseUnit = false;
            string nameBase;
            List<string> allUnits = new List<string>();
            List<string> quantityClasses = new List<string>();
            isbaseUnit = false;
            nameBase = "";
            string postFinal;
            postFinal = string.Empty;

            if (allUnits.Count == 0)
            {
                allUnits.AddRange(units.ListAllUOM<string>());
            }
            if (!allUnits.Contains(fromUnit))
            {
                return ("fromUnit not found or is not convertable!");
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
                return ("toUnit not found or is not convertable!");
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
            return postFinal;
        }

    }
}
