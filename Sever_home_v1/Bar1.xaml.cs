using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Shapes;

namespace Sever_home_v1
{
    /// <summary>
    /// Логика взаимодействия для Bar1.xaml
    /// </summary>
    public partial class Bar1 : Window
    {
        double[] chartOne = new double[10];
        string[] chartOneData = new string[10];
        double sum = 0;
        public Bar1()
        {

            InitializeComponent();
            //int twe = MainWindow.num;
            Array.Copy(MainWindow.CoMass, chartOne, MainWindow.CoMass.Length);
            Array.Copy(MainWindow.CoMassData, chartOneData, MainWindow.CoMassData.Length);

            // MainWindow.CoMass[];   ;
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "mAp Sun Solar",
                  Values = new ChartValues<double> { chartOne[0], chartOne[1], chartOne[2], chartOne[3], chartOne[4], chartOne[5], chartOne[6], chartOne[7], chartOne[8], chartOne[9] }


                },
               };
            Labels = new[] { chartOneData[0], chartOneData[1], chartOneData[2], chartOneData[3], chartOneData[4], chartOneData[5], chartOneData[6], chartOneData[7], chartOneData[8], chartOneData[9], };
           

            YFormatter = value => value.ToString("C");
            XFormatter = val => new DateTime((long)val).ToString("yyyy");
            DataContext = this;
            summSun();

        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }

        private void ReloadGraf(object sender, RoutedEventArgs e)
        {
            /*
            Array.Copy(MainWindow.CoMass, chartOne, MainWindow.CoMass.Length);
            Array.Copy(MainWindow.CoMassData, chartOneData, MainWindow.CoMassData.Length);

            Application.Current.Dispatcher.Invoke(() =>
            {


                SeriesCollection.Add(new LineSeries
                {
                    Title = "mAp Sun Solar",
                    Values = new ChartValues<double> { chartOne[0], chartOne[1], chartOne[2], chartOne[3], chartOne[4], chartOne[5], chartOne[6], chartOne[7], chartOne[8], chartOne[9] },
                    //LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                    // PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                    //  PointGeometrySize = 50,
                    //  PointForeground = Brushes.Gray
                });
               
                //Labels  = new [] { chartOneData[0], chartOneData[1], chartOneData[2], chartOneData[3], chartOneData[4], chartOneData[5], chartOneData[6], chartOneData[7], chartOneData[8], chartOneData[9] };
                Labels = new[] { chartOneData[0], chartOneData[1], chartOneData[2], chartOneData[3], chartOneData[4], chartOneData[5], chartOneData[6], chartOneData[7], chartOneData[8], chartOneData[9], };

                
                DataContext = this;
                SeriesCollection.RemoveAt(0);
                //modifying any series values will also animate and update the chart
                // SeriesCollection[0].Values.Add(5d);
                summSun();
                DataContext = this;

            });
            
    */
            Bar1 barTabSwhov2 = new Bar1();

            
            barTabSwhov2.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

            double Top = this.Top;
            double Left = this.Left;
            barTabSwhov2.Left = this.Left;
            barTabSwhov2.Top = this.Top;
            barTabSwhov2.Show();
            this.Close(); // закрываем Form2 (this - текущая форма)

           
        }



        public void summSun()
        {
            sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += chartOne[i];

            }
            sum = sum / 10;

            Tex1.Text = "Средний вольтаж: " + sum + "mA";
    
    
    }

        
    }
}
    
