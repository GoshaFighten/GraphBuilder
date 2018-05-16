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
        public void AddCurve(ZedGraphControl graph, MyPoint[] data, string name, Color color)
        {
            GraphPane pane = graph.GraphPane;
            LineItem curve = pane.AddCurve(
                name,
                data.Select(p => Convert.ToDouble(p.X)).ToArray(),
                data.Select(p => Convert.ToDouble(p.Y)).ToArray(),
                color,
                SymbolType.None
            );
            graph.AxisChange();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dialog = new OpenFileDialogSingle();
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            AddCurve(zedGraphControl, MyMath.AggregateData(dialog.Data, dialog.Precision), string.IsNullOrEmpty(dialog.CurveName) ? "Parameter" : dialog.CurveName, dialog.CurveColor.IsEmpty ? Color.Blue : dialog.CurveColor);
            var fitResult = MyMath.NormalDistributionFitToSample(dialog.Data);
            barStaticItem1.Caption = "P: " + fitResult.GoodnessOfFit.Probability;
            AddCurve(zedGraphControl, MyMath.GetGraphDataForNormalDistribution(fitResult.Parameter(0).Value, fitResult.Parameter(1).Value), "Normal Distribution", Color.Red);
            VisualizeData(dialog.Data);
        }

        private void VisualizeData(double[] data) {            
            AddCurve(zedGraphControl1, MyMath.GetNormalDistributionGraphData(data), "F(normal)", Color.Blue);
            MyPoint[] FData = MyMath.GetFGraph(data);
            AddCurve(zedGraphControl1, FData, "F", Color.Green);
            var d = MyMath.CalcD(data);
            MyPoint[] fDDataMin = MyPoint.GetMinusGraph(FData, d);
            MyPoint[] fDDdataMax = MyPoint.GetPlusGraph(FData, d);
            AddCurve(zedGraphControl1, fDDataMin, "D-", Color.Red);            
            AddCurve(zedGraphControl1, fDDdataMax, "D+", Color.Red);
            var dMaxGraph = MyMath.GetAllowedFMaxGraph(FData, d, 0.05);
            AddCurve(zedGraphControl1, dMaxGraph, "Allowed+", Color.Black);
            var dMinGraph = MyMath.GetAllowedFMinGraph(FData, d, 0.05);
            AddCurve(zedGraphControl1, dMinGraph, "Allowed-", Color.Black);
        }
    }
}
