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
    public partial class SeriesForm : Form
    {
        public SeriesForm()
        {
            InitializeComponent();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dialog = new OpenFileDialogSeries();
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            AddCurve(MyMath.WindowTest(dialog.Data, dialog.Window), string.IsNullOrEmpty(dialog.CurveName) ? "P-value" : dialog.CurveName, Color.Blue);
            AddHLine(0.05, "Confidence Interval", Color.Red);
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

        public void AddHLine(double y, string name, Color color)
        {
            GraphPane pane = zedGraphControl.GraphPane;
            LineItem line = new LineItem(
                name,
                new[] { pane.XAxis.Scale.Min, pane.XAxis.Scale.Max },
                new[] { y, y },
                color,
                SymbolType.None
            );
            line.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            line.Line.Width = 1f;
            pane.CurveList.Add(line);
            zedGraphControl.AxisChange();
        }
    }
}
