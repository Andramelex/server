using LiveCharts;
using LiveCharts.Wpf;
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

namespace Sever_home_v1
{
    /// <summary>
    /// Логика взаимодействия для Bar1.xaml
    /// </summary>
    public partial class Bar1 : Window


    {
        static int nLong = 60;
        double[] chartOneHore = new double[nLong];
        string[] chartOneDataHore = new string[nLong];
        int dLong = 24;


       double[] chartOne = new double[60];
      //  string[] chartOneData = new string[60];
        double sum = 0;
       // static int nLong = 60;
       // double[] chartOneHore = new double[nLong];
       // string[] chartOneDataHore = new string[nLong];
       // int dLong = 24;
        public Bar1()
        {
           
         
            InitializeComponent();
            chartOneHore[0] = 0;
            LoadDataDay(@"D:\SomeDir\data_soc_6.txt");
            // summSun();
            PuttGraf();

        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }

        public void PuttGraf()
        {

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "mAp Sun Solar",

                  Values = new ChartValues <double> {chartOneHore[0], chartOneHore[1], chartOneHore[2], chartOneHore[3], chartOneHore[4], chartOneHore[5], chartOneHore[6], chartOneHore[7], chartOneHore[8],
                   chartOneHore[9], chartOneHore[10], chartOneHore[11], chartOneHore[12], chartOneHore[13], chartOneHore[14], chartOneHore[15], chartOneHore[16], chartOneHore[17], chartOneHore[18], chartOneHore[19]}

                }
            };

            Labels = new string[nLong];


            YFormatter = value => value.ToString("C");
            XFormatter = val => new DateTime((long)val).ToString("yyyy");
            DataContext = this;
        }

        public void LoadDataDay(string path)
        {
           // LabelText.Text = "Данные за последний день";
            string line;
            double znachenie;
            double sum = 0;
            // int coint=0;
            for (int i = 0; i < dLong; i++)
            {
                List<string> lines = File.ReadLines(path).Reverse().Skip(60 * i).Take((60 * i) + 60).ToList();
                for (int z = 0; z < 60; z++)
                {
                    line = lines[z];
                    line = line.Replace(".", ",");
                    line = line.Replace("+", "");
                    line = line.Replace(";", "");
                    znachenie = Convert.ToDouble(line);
                    sum += znachenie;
                }
                Console.WriteLine(i + " сумма за час " + sum);
                sum = sum / 60;
                line = sum.ToString("0.##");

                chartOneHore[i] = Convert.ToDouble(line);




            }
            for (int i = 0; i < dLong; i++)
            {
                List<string> lines2 = File.ReadLines(@"D:\SomeDir\data_time.txt").Reverse().Skip(60 * i).Take((60 * i) + 1).ToList();
                line = lines2[i];
                line = line.Replace(";", "");
                chartOneDataHore[i] = line;
                Console.WriteLine(i + " способ даты LISTstring: " + lines2[i]);
            }

        }


        private void ReloadGraf(object sender, RoutedEventArgs e)
        {
            string path = @"D:\SomeDir\data_soc_6.txt";
            LoadData(path);
            clearValues();

        }



        public void summSun(int key)
        {
            sum = 0;
            for (int i = 0; i < key; i++)
            {
                sum += chartOneHore[i];

            }
            sum = sum / key;
            sum = Math.Round(sum, 3);
            Tex1.Text = "Средний вольтаж: " + sum + "mA";
            

        }
        public void LoadData(string path)
        {
           // LabelText.Text = "Данные за последний час";
            string line;
            List<string> lines = File.ReadLines(path).Reverse().Take(nLong).ToList();
            //Console.WriteLine(" мы прочитали: " + lines);

            for (int i = 0; i < nLong; i++)
            {
                line = lines[i];
                // Console.WriteLine(" мы прочитали последную строку: " + line);
                line = line.Replace(".", ",");
                line = line.Replace("+", "");
                line = line.Replace(";", "");
                //Console.WriteLine(i + "лайн равен: " + line);
                chartOneHore[i] = Convert.ToDouble(line);

            }

            lines = File.ReadLines(@"D:\SomeDir\data_time.txt").Reverse().Take(nLong).ToList();
            {
                for (int i = 0; i < nLong; i++)
                {
                    line = lines[i];
                    line = line.Replace(";", "");
                    chartOneDataHore[i] = line;

                }
            }
        }
        private void LoadDataDay(object sender, RoutedEventArgs e)
        {
            string path = @"D:\SomeDir\data_soc_6.txt";
            LoadDataDay(path);
            clearValuesDay();
        }
        public void clearValues()
        {
            string[] animals = new string[nLong];
           
            Application.Current.Dispatcher.Invoke(() =>
            {

                SeriesCollection[0].Values.Clear();

                for (int i = 0; i < nLong; i++)
                {
                    SeriesCollection[0].Values.Add(chartOneHore[i]);
                    Console.WriteLine(i + " добавялем: " + chartOneHore[i]);
                   
                    animals[i] = chartOneDataHore[i];


                }
                Array.Copy(animals, Labels, nLong);
                summSun(nLong);
                tex2.Text = "Последний час";
            });

        }
        public void clearValuesDay()
        {
            string[] LablCopy = new string[nLong];
            Application.Current.Dispatcher.Invoke(() =>
            {
                SeriesCollection[0].Values.Clear();

                for (int i = 0; i < dLong; i++)
                {
                    SeriesCollection[0].Values.Add(chartOneHore[i]);
                    LablCopy[i] = chartOneDataHore[i];

                }
                Array.Copy(LablCopy, Labels, nLong);
                summSun(dLong);
                tex2.Text = "Прошедший день";
            });
        }
    }
    
}
    
