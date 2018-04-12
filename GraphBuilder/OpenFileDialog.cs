using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphBuilder
{
    public partial class OpenFileDialog : Form
    {
        public OpenFileDialog()
        {
            InitializeComponent();
        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }
            labelControl1.Text = openFileDialog1.FileName;
            char[] file;
            StringBuilder builder = new StringBuilder();

            using (StreamReader reader = File.OpenText(openFileDialog1.FileName))
            {
                file = new char[reader.BaseStream.Length];
                await reader.ReadAsync(file, 0, (int)reader.BaseStream.Length);
            }

            foreach (char c in file)
            {
                builder.Append(c);
            }
            var array = builder.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(i => Convert.ToDouble(i)).ToArray();
            Data = array;
        }
        double[] fData;
        public double[] Data
        {
            get
            {
                return fData;
            }
            set
            {
                fData = value;
                gridControl1.DataSource = fData;
            }
        }
        public double Precision
        {
            get
            {
                return Convert.ToDouble(textEdit1.Text);
            }
            set
            {
                textEdit1.EditValue = value;
            }
        }
        public Color CurveColor
        {
            get
            {
                return colorEdit1.Color;
            }
            set
            {
                colorEdit1.Color = value;
            }
        }
        public string CurveName
        {
            get
            {
                return textEdit2.Text;
            }
            set
            {
                textEdit2.Text = value;
            }
        }
    }
}
