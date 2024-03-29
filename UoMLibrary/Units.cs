﻿using System;
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
        public List<T> ListAllUOM<T>()
        {
            List<T> names = new List<T>();
            using (XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"))
            {
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
            }
            names.RemoveAt(0);
            return names;
        }

        public List<T> ListAllSameBaseUnit<T>(string baseunit, string textName, string addBase)
        {

            List<T> baseUnits = new List<T>();
            baseUnits.Add((T)Convert.ChangeType(addBase, typeof(T)));
            using (XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"))
            {
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
            }
            return baseUnits;
        }

        public string findTypeOfConversion(string baseunitName)
        {
            using (XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"))
            {
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
            }
            return baseunitName;
        }

        public string findAnnotation(string annotation)
        {
            using (XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"))
            {
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
            }
            return annotation;
        }

        public string checkIfOther(string baseunitName)
        {
            using (XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"))
            {
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
            }
            return baseunitName;
        }

        public string getBaseUnit(string baseunitName)
        {
            using (XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"))
            {
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
            }
            return baseunitName;
        }

        public string getBaseUnitName(string baseunitName, string annotation)
        {
            using (XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"))
            {
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
            }
            return "nobase";
        }

        public string conversionSequence(string fromUnit, double value, string toUnit)
        {
            FromToConversion conversion = new FromToConversion();
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
                allUnits.AddRange(ListAllUOM<string>());
            }
            if (!allUnits.Contains(fromUnit))
            {
                return ("fromUnit not found or is not convertable!");
            }

            string baseunit = getBaseUnit(fromUnit);
            if (baseunit.Contains("Base unit:"))
            {
                string result = String.Join(" ", baseunit.Split(' ').Skip(2));
                string annotation = findAnnotation(result);
                ListAllSameBaseUnit<string>(annotation, result, "Base unit: none").ForEach(i => quantityClasses.Add(i));
                isbaseUnit = true;

            }
            else
            {
                string baseUnitName = getBaseUnitName(fromUnit, baseunit);
                nameBase = baseUnitName;
                ListAllSameBaseUnit<string>(baseunit, fromUnit, baseUnitName).ForEach(i => quantityClasses.Add(i));
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
                string annotation2 = findAnnotation(toUnit);
                string toConversionName2 = findTypeOfConversion(toUnit);
                string resultfinal = conversion.convertToFinal(toUnit, num, toConversionName2, annotation2, selectedIndex);
                postFinal = resultfinal;
                isbaseUnit = true;
            }
            else if (!toUnit.Contains("Base unit: none"))
            {
                string fromConversionName = findTypeOfConversion(fromUnit);
                string toConversionName = findTypeOfConversion(toUnit);
                string from_result = conversion.convertToBase(fromUnit, num, fromConversionName);
                double newval;
                newval = double.Parse(from_result, CultureInfo.InvariantCulture);
                string annotation = findAnnotation(toUnit);

                string resultfinal = conversion.convertToFinal(toUnit, newval, toConversionName, annotation, selectedIndex);
                postFinal = resultfinal;
                isbaseUnit = false;

            }
            return postFinal;
        }

        public string conversionSequenceGUI(string fromUnit, string value, string toUnit, bool isbaseUnit, int selectedIndex)
        {
            double num = double.Parse(value, CultureInfo.InvariantCulture);
            string returnString;
            FromToConversion conversion = new FromToConversion();
            if (isbaseUnit && !toUnit.Contains("Base unit: none"))
            {
                string annotation2 = findAnnotation(toUnit);
                string toConversionName2 = findTypeOfConversion(toUnit);
                string resultfinal = conversion.convertToFinal(toUnit, num, toConversionName2, annotation2, selectedIndex);
                returnString = resultfinal;
                isbaseUnit = true;
                return returnString;
            }
            else if (!toUnit.Contains("Base unit: none"))
            {
                string fromConversionName = findTypeOfConversion(fromUnit);
                string toConversionName = findTypeOfConversion(toUnit);
                string from_result = conversion.convertToBase(fromUnit, num, fromConversionName);
                double newval;
                newval = double.Parse(from_result, CultureInfo.InvariantCulture);
                string annotation = findAnnotation(toUnit);

                string resultfinal = conversion.convertToFinal(toUnit, newval, toConversionName, annotation, selectedIndex);
                returnString = resultfinal;
                isbaseUnit = false;
                return returnString;
            }
            return "Conversion failed!";
        }

    }
    public class FromToConversion : Units
    {
            public string convertToBase(string baseunitName, double value, string conversionType)
            {
            using (XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"))
            {
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
            }
                return null;
            }

            public string convertToFinal(string baseunitName, double value, string conversionType, string annotation, int selectedIndex)
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
                using (XmlReader xmlReader = XmlReader.Create(@"C:\Users\alexa\source\repos\UnitsOfMeasure\UoMLibrary\dataUnits.xml"))
                {
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
                }
                return "";
            }
        }

}