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
    public partial class SingleForm : Form
    {
        public SingleForm()
        {
            InitializeComponent();
        }
        public void AddCurve(MyPoint[] data, string name, Color color)
        {
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
            var dialog = new OpenFileDialogSingle();
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            AddCurve(MyMath.AggregateData(dialog.Data, dialog.Precision), string.IsNullOrEmpty(dialog.CurveName) ? "Parameter" : dialog.CurveName, dialog.CurveColor.IsEmpty ? Color.Blue : dialog.CurveColor);
            var fitResult = MyMath.NormalDistributionFitToSample(dialog.Data);
            barStaticItem1.Caption = "P: " + fitResult.GoodnessOfFit.Probability;
            AddCurve(MyMath.GetGraphDataForNormalDistribution(fitResult.Parameter(0).Value, fitResult.Parameter(1).Value), "Normal Distribution", Color.Red);
        }
    }
}
