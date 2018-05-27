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

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var dialog = new OpenFileDialogSeries();
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK) {
                return;
            }
            var result1 = MyMath.WindowTestMyLogic(dialog.Data, dialog.Window);
            zedGraphControl.GraphPane.Title.Text = dialog.CurveName;
            zedGraphControl1.GraphPane.Title.Text = dialog.CurveName;
            AddCurve(zedGraphControl, result1, "Probability", Color.Blue);
            AddHLine(zedGraphControl, 0.05, "Alpha", Color.Red, result1[0].X, result1[result1.Length - 1].X);
            var result2 = MyMath.WindowTestNewLogic(dialog.Data, dialog.Window);
            AddCurve(zedGraphControl1, result2.Item1, "DMax", Color.Blue);
            AddCurve(zedGraphControl1, result2.Item2, "Dn", Color.Red);

            var alphas = result1.Select(p => new MyPoint() { X = p.X, Y = 0.05 }).ToArray();
            GetZones(Tuple.Create(alphas, result1), zedGraphControl);
            GetZones(result2, zedGraphControl1);
            zedGraphControl.Refresh();
            zedGraphControl1.Refresh();
        }

        private void GetZones(Tuple<MyPoint[], MyPoint[]> data, ZedGraphControl zedGraphControl) {
            var sign = -1;
            var areas = new List<Area>();
            var currentArea = new Area() {
                MinX = data.Item1[0].X,
                Type = AreaType.Negative
            };
            areas.Add(currentArea);
            for (int i = 0; i < data.Item1.Length; i++) {
                var maxD = data.Item1[i].Y;
                var dn = data.Item2[i].Y;
                var currentSign = Math.Sign(dn - maxD);
                if (sign != currentSign) {
                    currentArea.MaxX = data.Item1[i].X;
                    currentArea = new Area() {
                        MinX = data.Item1[i].X,
                        Type = currentSign > 0 ? AreaType.Positive : AreaType.Negative
                    };
                    areas.Add(currentArea);
                    sign = currentSign;
                }
            }
            currentArea.MaxX = data.Item1[data.Item1.Length - 1].X;

            foreach (var area in areas) {
                BoxObj box = new BoxObj(area.MinX, zedGraphControl.GraphPane.YAxis.Scale.Max, area.MaxX - area.MinX, zedGraphControl.GraphPane.YAxis.Scale.Max, Color.Empty, area.Type == AreaType.Positive ? Color.LightGreen : Color.IndianRed);
                box.Location.CoordinateFrame = CoordType.AxisXYScale;
                box.Location.AlignH = AlignH.Left;
                box.Location.AlignV = AlignV.Top;
                box.ZOrder = ZOrder.E_BehindCurves;
                zedGraphControl.GraphPane.GraphObjList.Add(box);
            }
        }

        public void AddCurve(ZedGraphControl zedGraphControl, MyPoint[] data, string name, Color color)
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

        public void AddHLine(ZedGraphControl zedGraphControl, double y, string name, Color color, double minX, double maxX)
        {
            GraphPane pane = zedGraphControl.GraphPane;
            LineItem line = new LineItem(
                name,
                new[] { minX, maxX },
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
