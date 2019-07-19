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
    /// Логика взаимодействия для WinGraphTab.xaml
    /// </summary>
    public partial class WinGraphTab : Window
    {
        public WinGraphTab()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.SendtoCom("2");
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.SendtoCom("3");
        }

        

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            MainWindow.SendtoCom("LedOn");
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            MainWindow.SendtoCom("LedOff"); 
        }
        string[] splitData;
        private void LoaData(object sender, RoutedEventArgs e)
        {
            try { 
            MainWindow.SendtoCom("1");
            //port.Write("1");
            System.Threading.Thread.Sleep(1000);
            string returnMessage = MainWindow.port.ReadLine();
            //TextBox.Text = returnMessage;
            //returnMessage = "12;13;14;15;16;";
            splitData = returnMessage.Split(';');
            Textbox_hub_th.Text = ("T: " + splitData[1] + "  H: " + splitData[2]);
            Textbox_zzz_co.Text = ("Co: " + splitData[4]);
            Textbox_zzz_th.Text = ("T: " + splitData[6] + "  H: " + splitData[7]);
            //TextBox.Text = splitData;
            Console.WriteLine("было :" + returnMessage);
            Console.WriteLine("Получили :" + splitData[1]);
            splitData = MainWindow.datasoketToGraf.Split(';');
            Textbox_forest_tH.Text = ("T: " + splitData[1] + "  H: " + splitData[5]);
            Textbox_forest_co.Text = ("Co: " + splitData[3] + "  tCO: " + splitData[4]);
            Textbox_forest_solar.Text = ("Solar: " + splitData[6] + "mA" + "  V :" + splitData[7] + "v");
        }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка  :" + ex);
            }
        }

      
        private void Button_Click_clearLog(object sender, RoutedEventArgs e)
        {
            clearLog();

        }

        private void clearLog()
        {
           MainWindow.LongLogCut();
            MessageBox.Show("Логи обрезаны");
            
        }
    }
}
