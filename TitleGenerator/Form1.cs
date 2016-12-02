using System;
using System.Windows.Forms;

namespace TitleGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tool = new TeklaTools();
            textBox1.Text = tool.GetAssemblyNumbers();
            label1.Text = tool.GetInfo();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var tool = new TeklaTools();
            textBox2.Text = tool.GetMultiInfo();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string number = textBox3.Text;

            var tool = new TeklaTools();
            tool.WriteNumber(number);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var tool = new TeklaTools();
            textBox4.Text = tool.ReturnFullName();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            label4.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\drawings.csv";
            var tool = new TeklaTools();
            tool.ReturnFullNameToTextFile();
        }
    }
}
