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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = "Excel spreadsheet (*.xlsx)|*.xlsx";

            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                SettingsGrid.Visibility = Visibility.Visible;
                source = fileDialog.FileName;
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

        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            if (Slider.Value == 1) 
            {
                var mw = new MainViewModel(
                    source,
                    int.Parse(FirstColumnStartBox.Text),
                    int.Parse(FirstRowStartBox.Text),
                    int.Parse(FirstlemensNumber.Text),
                    int.Parse(SecondColumnStartBox.Text),
                    int.Parse(SecondRowStartBox.Text),
                    int.Parse(SecondElemensNumber.Text)
                    );
                GraphView.Model = mw.MyModel;
            }
            else
            {
                var mw = new MainViewModel(
                    source,
                    int.Parse(FirstColumnStartBox.Text),
                    int.Parse(FirstRowStartBox.Text),
                    int.Parse(FirstlemensNumber.Text)
                    );

                GraphView.Model = mw.MyModel;
            }
        }
    }

    public class MainViewModel
    {
        const double dx = 0.001;
        const double pointSize = 2;

        public MainViewModel(string path, int column, int row, int number)
        {
            MyModel = new PlotModel { Title = "Один клас" };
            MyModel.PlotType = PlotType.Cartesian;
            ExcelParser ep = new ExcelParser(
                path,
                0,
                number,
                column,
                column + 1,
                row,
                row
                );

            ObjectClass oc = new ObjectClass(ep.Result);
            ScatterSeries points = new ScatterSeries() { Title = "Клас" };
            FunctionSeries hyperPlane = new FunctionSeries(
                GetFunc(oc.HyperPlane),
                oc.MinX,
                oc.MaxX,
                dx,
                "Гіперплощина"
                );
                

            foreach (var point in ep.Result)
            {
                points.Points.Add(new ScatterPoint(point.X, point.Y, pointSize));
            }

            MyModel?.Series.Add(points);

            MyModel?.Series.Add(hyperPlane);
        }

        public MainViewModel(
            string path, 
            int columnFirst, 
            int rowFirst, 
            int numberFirst,
            int columnSecond,
            int rowSecond,
            int numberSecond
            )
        {
            MyModel = new PlotModel { Title = "Два класи" };
            MyModel.PlotType = PlotType.Cartesian;
            ExcelParser epFirst = new ExcelParser(
                path,
                0,
                numberFirst,
                columnFirst,
                columnFirst + 1,
                rowFirst,
                rowFirst
                );

            ObjectClass ocFirst = new ObjectClass(epFirst.Result);
            ScatterSeries pointsFirst = new ScatterSeries() { Title = "Клас 1" };
            FunctionSeries hyperPlaneFirst = new FunctionSeries(
                GetFunc(ocFirst.HyperPlane),
                ocFirst.MinX,
                ocFirst.MaxX,
                dx,
                "Гіперплощина класу 1"
                );


            foreach (var point in epFirst.Result)
            {
                pointsFirst.Points.Add(new ScatterPoint(point.X, point.Y, pointSize));
            }

            MyModel?.Series.Add(pointsFirst);
            MyModel?.Series.Add(hyperPlaneFirst);

            ExcelParser epSecond = new ExcelParser(
                path,
                0,
                numberSecond,
                columnSecond,
                columnSecond + 1,
                rowSecond,
                rowSecond
                );

            ObjectClass ocSecond = new ObjectClass(epSecond.Result);
            ScatterSeries pointsSecond = new ScatterSeries() { Title = "Клас 2" };
            FunctionSeries hyperPlaneSecond = new FunctionSeries(
                GetFunc(ocSecond.HyperPlane),
                ocSecond.MinX,
                ocSecond.MaxX,
                dx,
                "Гіперплощина класу 2"
                );


            foreach (var point in epSecond.Result)
            {
                pointsSecond.Points.Add(new ScatterPoint(point.X, point.Y, pointSize));
            }

            MyModel?.Series.Add(pointsSecond);
            MyModel?.Series.Add(hyperPlaneSecond);

            Line hyper = ObjectClass.Get2ClassesHyperPlaneAsSVM(ocFirst, ocSecond);
            FunctionSeries hyperplane = new FunctionSeries(
                GetFunc(hyper),
                Math.Min(ocFirst.MinX, ocSecond.MinX),
                Math.Max(ocFirst.MaxX, ocSecond.MaxX),
                dx,
                "Гіперплощина між класами"
                );
            MyModel?.Series.Add(hyperplane);
        }

        private Func<double, double> GetFunc(Line l)
        {
            return new Func<double, double>(x => (l.A / -l.B) * x + l.C / -l.B);
        }

        public PlotModel MyModel { get; private set; }
    }
}
