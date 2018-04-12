using GraphBuilder.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace GraphBuilder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        public void AddCurve(double[] source, double precision, string name, Color color)
        {
            var data = MyMath.AggregateData(source, precision);
            GraphPane pane = zedGraphControl.GraphPane;
            LineItem curve = pane.AddCurve(
                name,
                data.Select(p => Convert.ToDouble(p.X)).ToArray(),
                data.Select(p => Convert.ToDouble(p.Y)).ToArray(),
                color,
                SymbolType.None
            );
            zedGraphControl.AxisChange();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                return;
            }
            AddCurve(dialog.Data, dialog.Precision, dialog.CurveName, dialog.CurveColor);
        }
    }
}
