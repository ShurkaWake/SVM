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
        public MainWindow()
        {
            InitializeComponent();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var mvm = new MainViewModel();
            GraphView.Model = mvm.MyModel;
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
            this.MyModel.Series.Add(ars);
        }

        public PlotModel MyModel { get; private set; }
    }
}
