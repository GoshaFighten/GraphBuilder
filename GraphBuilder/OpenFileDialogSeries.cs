using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphBuilder
{
    public partial class OpenFileDialogSeries : Form
    {
        public OpenFileDialogSeries()
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

        public int Window
        {
            get
            {
                return (int)textEdit1.Value;
            }
            set
            {
                textEdit1.EditValue = value;
            }
        }
    }
}
