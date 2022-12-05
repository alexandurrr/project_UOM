using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Generic;
using System.Globalization;
using UoMLibrary;

namespace UoMGUI
{
    public partial class GUI : Form
    {
        bool isbaseUnit = false;
        string nameBase;
        public GUI()
        {
            InitializeComponent();
            IUnits units = new Units();
            IxmlReader xmlReader = new xmlReader();
            xmlReader.QuantityUnits<string>().ForEach(i => comboBox1.Items.Add(i));
            units.ListAllUOM<string>().ForEach(i => comboBox3.Items.Add(i));
            textBox3.Text = xmlReader.ListUnitDimensions();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            IxmlReader xmlReader = new xmlReader();
            xmlReader.ListAllUOMforQC<string>(comboBox1.Text).ForEach(i => comboBox2.Items.Add(i));
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Items.Clear();
            IxmlReader xmlReader = new xmlReader();
            xmlReader.FindAliasesforUOM<string>(comboBox2.Text).ForEach(i => comboBox5.Items.Add(i));

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            isbaseUnit = false;
            nameBase = "";
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
                nameBase = baseUnitName;
                units.ListAllSameBaseUnit<string>(baseunit, comboBox3.Text, baseUnitName).ForEach(i => comboBox4.Items.Add(i));
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            IUnits units = new Units();
            int selectedIndex = 0;
            if (nameBase == comboBox4.Text && comboBox4.SelectedIndex == 0){selectedIndex = 1;}

            if (textBox1.Text.Trim().Length == 0 || comboBox3.Text.Trim().Length == 0 || comboBox4.Text.Trim().Length == 0)
            {
                MessageBox.Show("Need values!");
            }

            textBox2.Text = units.conversionSequenceGUI(comboBox3.Text, textBox1.Text, comboBox4.Text, isbaseUnit, selectedIndex);            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != 'E' && e.KeyChar != '+')
                e.Handled = true;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}