using Microsoft.VisualBasic.ApplicationServices;
using System.Globalization;
using UoMLibrary;

namespace UoMGUI
{
    public partial class Form1 : Form
    {
        bool isbaseUnit = false;
        public Form1()
        {
            InitializeComponent();
            IUnits units = new Units();
            units.QuantityUnits<string>().ForEach(i => comboBox1.Items.Add(i));
            units.ListAllUOM<string>().ForEach(i => comboBox3.Items.Add(i));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            IUnits units = new Units();
            units.ListAllUOMforQC<string>(comboBox1.Text).ForEach(i => comboBox2.Items.Add(i));
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            IUnits units = new Units();
            string baseunit = units.getBaseUnit(comboBox3.Text);
            if (baseunit.Contains("Base unit:"))
            {
                string result = String.Join(" ", baseunit.Split(' ').Skip(2));
                string annotation = units.findAnnotation(result);
                units.ListAllSameBaseUnit<string>(annotation, result, "Base unit: none").ForEach(i => comboBox4.Items.Add(i));
                isbaseUnit = true;

            }
            else
            {
                string baseUnitName = units.getBaseUnitName(comboBox3.Text, baseunit);
                string addBase = String.Format("Base unit: {0}", baseUnitName);
                units.ListAllSameBaseUnit<string>(baseunit, comboBox3.Text, addBase).ForEach(i => comboBox4.Items.Add(i));
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            IUnits units = new Units();
            if (textBox1.Text.Trim().Length == 0 || comboBox3.Text.Trim().Length == 0 || comboBox4.Text.Trim().Length == 0)
            {
                MessageBox.Show("Need values!");
            }
            else
            {
                double num = double.Parse(textBox1.Text, CultureInfo.InvariantCulture);
                if (isbaseUnit && !comboBox4.Text.Contains("Base unit: none"))
                {
                    string annotation2 = units.findAnnotation(comboBox4.Text);
                    string toConversionName2 = units.findTypeOfConversion(comboBox4.Text);
                    string resultfinal = units.convert_to_final(comboBox4.Text, num, toConversionName2, annotation2);
                    textBox2.Text = resultfinal;
                    isbaseUnit = true;
                }
                else if (!comboBox4.Text.Contains("Base unit: none"))
                {
                    string fromConversionName = units.findTypeOfConversion(comboBox3.Text);
                    string toConversionName = units.findTypeOfConversion(comboBox4.Text);
                    string from_result = units.convertToBase(comboBox3.Text, num, fromConversionName);
                    double newval;
                    newval = double.Parse(from_result, CultureInfo.InvariantCulture);
                    string annotation = units.findAnnotation(comboBox4.Text);

                    string resultfinal = units.convert_to_final(comboBox4.Text, newval, toConversionName, annotation);
                    textBox2.Text = resultfinal;
                    isbaseUnit = false;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != 'E' && e.KeyChar != '+')
                e.Handled = true;
        }
    }
}