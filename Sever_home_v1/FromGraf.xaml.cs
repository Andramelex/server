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
        static int nLong = 60;
        double[] chartOneHore = new double[nLong];
        string[] chartOneDataHore = new string[nLong];
        int dLong = 24;
        
        public FromGraf()
        {
            InitializeComponent();
            chartOneHore[0] = 0;
            LoadData(@"D:\SomeDir\data_2.txt");
            PuttGraf();
            CheckDataLog();
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
                    Title = "Мониторинг",

                  Values = new ChartValues <double> {chartOneHore[0], chartOneHore[1], chartOneHore[2], chartOneHore[3], chartOneHore[4], chartOneHore[5], chartOneHore[6], chartOneHore[7], chartOneHore[8],
                   chartOneHore[9], chartOneHore[10], chartOneHore[11], chartOneHore[12], chartOneHore[13], chartOneHore[14], chartOneHore[15], chartOneHore[16], chartOneHore[17], chartOneHore[18], chartOneHore[19]}
                
                }
            };

            Labels = new string[nLong];

            
            YFormatter = value => value.ToString("C");
            XFormatter = val => new DateTime((long)val).ToString("yyyy");
            DataContext = this;
        }
        public void CheckDataLog() {
            string path = @"D:\SomeDir\data_2.txt";
            List<string> linesCut = File.ReadLines(path).ToList();
            int checkday = linesCut.Count;
           // Console.WriteLine(" логи дней равны: " + checkday);
           
            double zz23 =Convert.ToDouble(checkday);
                      

            //------------------Вот этот метод позовляет изменять Юи в главном потоке из дочерхних потоков
            Application.Current.Dispatcher.Invoke(() =>
            {
                zz23 = zz23 / 1444;
                string result2 = String.Format("{0:f2}", zz23);
               
                ChedDayLog.Content = result2; 



            });
            //-------------Вот этот метод позовляет изменять Юи в главном потоке из дочерхних потоков
        }
        public void LoadData(string path)
        {
            LabelText.Text = "Данные за последний час";
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
                for (int i = 0; i < nLong; i++) {
                    line = lines[i];
                    line = line.Replace(";", "");
                    chartOneDataHore[i] = line;
                   
                }
            }
        }

        public void LoadDataDay(string path) {
            LabelText.Text = "Данные за последний день";
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
            for (int i = 0; i < dLong; i++) {
                List<string> lines2 = File.ReadLines(@"D:\SomeDir\data_time.txt").Reverse().Skip(60 * i).Take((60 * i) + 1).ToList();
                line = lines2[i];
                line = line.Replace(";", "");
                chartOneDataHore[i] = line;
                Console.WriteLine(i + " способ даты LISTstring: " + lines2[i]);
            }

        }

        public void clearValues()
        {
            string[] animals = new string[nLong];
            List<string> LabelAdd = new List<string>();
            Application.Current.Dispatcher.Invoke(() =>
                 {
                      
                      SeriesCollection[0].Values.Clear(); 
                                      
                     for (int ura = 0; ura < nLong; ura++)
                     {
                         SeriesCollection[0].Values.Add(chartOneHore[ura]);
                         Console.WriteLine(ura + " добавялем: " + chartOneHore[ura]);
                         LabelAdd.Add(chartOneDataHore[ura]);
                         animals[ura] = chartOneDataHore[ura];


                     }
                     Array.Copy(animals, Labels, nLong);
                    // Labels = animals;
                 });
 
        }
        public void clearValuesDay() {
            string[] LablCopy = new string[nLong];
            Application.Current.Dispatcher.Invoke(() =>
            {
                SeriesCollection[0].Values.Clear();
                
                for (int i=0; i< dLong; i++)
                {
                    SeriesCollection[0].Values.Add(chartOneHore[i]);
                    LablCopy[i] = chartOneDataHore[i];
                  
                }
                Array.Copy(LablCopy, Labels, nLong);

            });
        }

        private void SetLoad_1(object sender, RoutedEventArgs e) // Влажность хаб ЧАС
        {
            
            string path = @"D:\SomeDir\data_2.txt";
            LoadData(path);
            clearValues();
        }

        private void SetLoad_2(object sender, RoutedEventArgs e) // Сo2 Спальня ЧАС
        {
           
            string path = @"D:\SomeDir\data_5.txt";
            LoadData(path);
            clearValues();
        }

        private void SetLoad_3(object sender, RoutedEventArgs e) // Темп хаб ЧАС
        {
            string path = @"D:\SomeDir\data_3.txt";
            LoadData(path);
            clearValues();
        }

       
        private void SetLoad_6(object sender, RoutedEventArgs e)// влажность, спальня ЧАС
        {
            string path = @"D:\SomeDir\data_7.txt";
            LoadData(path);
            clearValues();
        }

        private void Butt_temoZZ(object sender, RoutedEventArgs e) // Темп, спальня ЧАС
        {
            string path = @"D:\SomeDir\data_8.txt";
            LoadData(path);
            clearValues();
        }

        private void SetLoad_5(object sender, RoutedEventArgs e) // Со, новое (лес) ЧАС
        {
            string path = @"D:\SomeDir\data_soc_3.txt";
            LoadData(path);
            clearValues();
        }

        private void day_set1(object sender, RoutedEventArgs e)  // Со2 старое  весь день
        {
            string path = @"D:\SomeDir\data_5.txt";
            LoadDataDay(path);
            clearValuesDay();
        }

        private void day_set2(object sender, RoutedEventArgs e) // Со, новое (лес) весь день
        {
            string path = @"D:\SomeDir\data_soc_3.txt";
            LoadDataDay(path);
            clearValuesDay();
        }

        private void day_set3(object sender, RoutedEventArgs e) // Влажность (спальня) весь день
        {
            string path = @"D:\SomeDir\data_7.txt";
            LoadDataDay(path);
            clearValuesDay();

        }

        private void day_set4(object sender, RoutedEventArgs e) // Темерат (спальня) весь день
        {
            string path = @"D:\SomeDir\data_8.txt";
            LoadDataDay(path);
            clearValuesDay();
        }

        private void day_set5(object sender, RoutedEventArgs e) // Влажность хаб весь день
        {
            string path = @"D:\SomeDir\data_2.txt";
            LoadDataDay(path);
            clearValuesDay();
        }

        private void day_set6(object sender, RoutedEventArgs e) // температура хаб весь день
        {
            string path = @"D:\SomeDir\data_3.txt";
            LoadDataDay(path);
            clearValuesDay();

        }
    }
}

