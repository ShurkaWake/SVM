using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using OxyPlot;
using OxyPlot.Series;
using ORO_Lb4.Models;

namespace ORO_Lb4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mvm = new MainViewModel();
            GraphView.Model = mvm.MyModel;
        }
    }

    public class MainViewModel
    {
        public MainViewModel()
        {
            Line line = new Line(new Point(0, 2), new Point(-4, -2));
            Func<double, double> lineFunc = x => line.A * x + line.C;
            Line line1 = new Line(new Point(-6, -10), new Point(3, -6));
            Func<double, double> lineFunc1 = x => line1.A * x + line1.C;
            Line line2 = Line.GetMiddleLine(line, line1);
            Func<double, double> lineFunc2 = x => line2.A * x + line2.C;

            this.MyModel = new PlotModel { Title = "Example 1" };
            this.MyModel.Series.Add(new FunctionSeries(lineFunc, -10, 10, 0.1, "line"));
            this.MyModel.Series.Add(new FunctionSeries(lineFunc1, -10, 10, 0.1, "line1"));
            this.MyModel.Series.Add(new FunctionSeries(lineFunc2, -10, 10, 0.1, "line2"));

            var ars = new ScatterSeries();
            ars.Points.Add(new ScatterPoint(1, 2, 1));
            ars.Points.Add(new ScatterPoint(1, 4, 1));
            ars.Points.Add(new ScatterPoint(3, 4, 1));
            this.MyModel.Series.Add(ars);
        }

        public PlotModel MyModel { get; private set; }
    }
}
