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
            ObjectClass oc = new ObjectClass(ep.Result);
            Func<double, double> f = new Func<double, double>(x => oc.HyperSquare.A * x + oc.HyperSquare.C);
            var fSeries = new FunctionSeries(f, oc.MinX, oc.MaxX, 0.001, "Test");

            this.MyModel.Series.Add(ars);
            this.MyModel.Series.Add(fSeries);
        }

        public PlotModel MyModel { get; private set; }
    }
}
