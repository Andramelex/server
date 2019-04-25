using System;
using System.Collections.Generic;
using System.IO;
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
using LiveCharts;
using LiveCharts.Wpf;

namespace Sever_home_v1
{
    /// <summary>
    /// Логика взаимодействия для FromGraf.xaml
    /// </summary>
    public partial class FromGraf : Window
    {
        double[] chartOneHore = new double[5];
        string[] chartOneDataHore = new string[5];
        public FromGraf()
        {
            InitializeComponent();
            chartOneHore[0] = 0;
        }
       
     
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }

        public void PuttGraf()
        {
            string path = @"D:\SomeDir\data_2.txt";
           
            LoadData(path);
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "первый график",
                  Values = new ChartValues <double> {chartOneHore[0], chartOneHore[1], chartOneHore[2], chartOneHore[3], chartOneHore[4], chartOneHore[5]}
                }
            }; 
            Labels = new[] { chartOneDataHore[0], chartOneDataHore[1], chartOneDataHore[2], chartOneDataHore[3], chartOneDataHore[4], chartOneDataHore[5]};


            YFormatter = value => value.ToString("C");
            XFormatter = val => new DateTime((long)val).ToString("yyyy");
            DataContext = this;
        }
       // public void
        public void LoadData(string path) {
            
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                int contLine = 0;
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    line = line.Replace(".", ",");
                    line = line.Replace("+", "");
                    line = line.Replace(";", "");
                    chartOneHore[contLine] = Convert.ToDouble(line); 
                 contLine ++;
                        }

            }
            
            using (StreamReader sr = new StreamReader(@"D:\SomeDir\data_time.txt", System.Text.Encoding.Default))
            {
                int contLine = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace(";", "");
                    chartOneDataHore[contLine] =line;
                    contLine++;
                }

            }

        }

    }
}

