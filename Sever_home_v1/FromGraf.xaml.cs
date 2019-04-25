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
        int dataLock = 0;
        
        public FromGraf()
        {
            InitializeComponent();
            chartOneHore[0] = 0;
            LoadData(@"D:\SomeDir\data_2.txt");
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
                    Title = "Последний час",

                  Values = new ChartValues <double> {chartOneHore[0], chartOneHore[1], chartOneHore[2], chartOneHore[3], chartOneHore[4], chartOneHore[5], chartOneHore[6], chartOneHore[7], chartOneHore[8],
                   chartOneHore[9], chartOneHore[10], chartOneHore[11], chartOneHore[12], chartOneHore[13], chartOneHore[14], chartOneHore[15], chartOneHore[16], chartOneHore[17], chartOneHore[18], chartOneHore[19]}
                
                }
            };

            Labels = new string[nLong];

            
            YFormatter = value => value.ToString("C");
            XFormatter = val => new DateTime((long)val).ToString("yyyy");
            DataContext = this;
        }
       // public void
        public void LoadData2(string path) {
            
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                int contLine = 0;
                string line;
               
                // while((line = sr.ReadLine()) != null)
                while (contLine< nLong)
                {
                    line = sr.ReadLine();
                    line = line.Replace(".", ",");
                    line = line.Replace("+", "");
                    line = line.Replace(";", "");
                    Console.WriteLine(contLine+"лайн равен: " + line);
                    chartOneHore[contLine] = Convert.ToDouble(line); 
                 contLine ++;
                        }

            }
            
            using (StreamReader sr = new StreamReader(@"D:\SomeDir\data_time.txt", System.Text.Encoding.Default))
            {
                int contLine = 0;
                string line;
                //while ((line = sr.ReadLine()) != null)
                while (contLine < nLong)
                {
                    line = sr.ReadLine();
                    line = line.Replace(";", "");
                    chartOneDataHore[contLine] =line;
                    contLine++;
                }

            }
           

        }
        public void LoadData(string path)
        {
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
        private void SetLoad_1(object sender, RoutedEventArgs e)
        {
            
            string path = @"D:\SomeDir\data_2.txt";
            LoadData(path);
            clearValues();
        }

        private void SetLoad_2(object sender, RoutedEventArgs e)
        {
           
            string path = @"D:\SomeDir\data_5.txt";
            LoadData(path);
            clearValues();
        }

        private void SetLoad_3(object sender, RoutedEventArgs e)
        {
            string path = @"D:\SomeDir\data_3.txt";
            LoadData(path);
            clearValues();
        }

       
        private void SetLoad_6(object sender, RoutedEventArgs e)
        {
            string path = @"D:\SomeDir\data_7.txt";
            LoadData(path);
            clearValues();
        }

        private void SetLoad_4(object sender, RoutedEventArgs e)
        {
            string path = @"D:\SomeDir\data_8.txt";
            LoadData(path);
            clearValues();
        }

        private void SetLoad_5(object sender, RoutedEventArgs e)
        {
            string path = @"D:\SomeDir\data_soc_3.txt";
            LoadData(path);
            clearValues();
        }
    }
}

