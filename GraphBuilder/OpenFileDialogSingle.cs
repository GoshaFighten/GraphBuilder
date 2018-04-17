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
    public partial class OpenFileDialogSingle : Form
    {
        public OpenFileDialogSingle()
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
            
            Data = await OpenHelper.ReadFile(openFileDialog1.FileName);
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
