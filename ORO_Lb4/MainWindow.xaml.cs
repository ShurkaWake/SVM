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
using ORO_Lb4.Entities;
using ORO_Lb4.DataAccess;

namespace ORO_Lb4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string source;
        public MainWindow()
        {
            InitializeComponent();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var mvm = new MainViewModel();
            GraphView.Model = mvm.MyModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = "Excel spreadsheet (*.xlsx)|*.xlsx";

            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                SettingsGrid.Visibility = Visibility.Visible;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Slider.Value == 1)
            {
                TwoClassesGrid.Visibility = Visibility.Visible;
            }
            else
            {
                TwoClassesGrid.Visibility = Visibility.Hidden;
            }
        }
    }

    public class MainViewModel
    {
        public MainViewModel()
        {
            this.MyModel = new PlotModel { Title = "Test" };

            var ars = new ScatterSeries();
            ExcelParser ep = new ExcelParser(
                @"C:\Users\Sasha\Desktop\ОРО_Лр_4_Табл\40.xlsx",
                0,
                680,
                0,
                1,
                0,
                0
                );
            foreach(var p in ep.Result)
            {
                ars.Points.Add(new ScatterPoint(p.X, p.Y, 2));
            }

            var ars1 = new ScatterSeries();
            ExcelParser ep1 = new ExcelParser(
                @"C:\Users\Sasha\Desktop\ОРО_Лр_4_Табл\40.xlsx",
                0,
                680,
                4,
                5,
                0,
                0
                );

            foreach (var p in ep1.Result)
            {
                ars1.Points.Add(new ScatterPoint(p.X, p.Y, 2));
            }

            ObjectClass oc = new ObjectClass(ep.Result);
            Func<double, double> f = new Func<double, double>(x => oc.HyperPlane.A * x + oc.HyperPlane.C);
            ObjectClass oc1 = new ObjectClass(ep1.Result);
            Func<double, double> f1 = new Func<double, double>(x => oc1.HyperPlane.A * x + oc1.HyperPlane.C);

            var fSeries = new FunctionSeries(f, oc.MinX, oc.MaxX, 0.001, "Test");
            var fSeries1 = new FunctionSeries(f1, oc1.MinX, oc1.MaxX, 0.001, "Test1");

            this.MyModel.Series.Add(ars);
            this.MyModel.Series.Add(ars1);
            this.MyModel.Series.Add(fSeries);
            this.MyModel.Series.Add(fSeries1);

            var hp = ObjectClass.Get2ClassesHyperPlaneAsSVM(oc, oc1);
            Func<double, double> f2 = new Func<double, double>(x => hp.A * x + hp.C);
            var fSeries2 = new FunctionSeries(f2, oc.MinX, oc1.MaxX, 0.001, "SVM");
            MyModel.Series.Add(fSeries2);

            /*var hp1 = ObjectClass.Get2ClassesHyperPlaneAsAvg(oc, oc1);
            Func<double, double> f3 = new Func<double, double>(x => hp1.A * x + hp1.C);
            var fSeries3 = new FunctionSeries(f3, oc.MinX, oc1.MaxX, 0.001, "Avg");
            MyModel.Series.Add(fSeries3);*/
            MyModel.PlotType = PlotType.Cartesian;
        }

        public PlotModel MyModel { get; private set; }
    }
}
