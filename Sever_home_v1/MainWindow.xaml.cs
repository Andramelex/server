using System;

using System.Text;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Media.Imaging;

using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Drawing;


namespace Sever_home_v1
{
   
    public partial class MainWindow : Window
    {
        
       public static SerialPort port = new SerialPort();
        string[] ports = SerialPort.GetPortNames();
        // string oldtext = "";
        int statusPort = 0;
        
        public int countsolarMassive = 0;
        public int num = 0;
        //DateTime date1 = new DateTime();
        private static System.Timers.Timer aTimer;
        int chekCam = 0;
        static int Sport = 2121; // порт для приема входящих запросов
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("192.168.1.155"), Sport); // получаем адреса для запуска сокета
        string[] splitData;
        string[] splitDataSoc;
        string returnMessage;
        string data = null;
        string data1 = null;
        Socket handler;
        int OtvetSocketInt = 0;
        int AvtDataCom = 0;
       // Uri uri = new Uri(@"D:\wamp2\www\3\images\cam\web1.jpg", UriKind.Absolute);
       // BitmapImage bitmap;
       // BitmapImage bitmap = new BitmapImage();

        //Image img = new Image();
        public static double[] CoMass = new double[10];
        public static string[] CoMassData = new string[10];
       
        public MainWindow()
        {
            CoMass[0] = 5;
            CoMass[1] = 5;
            CoMass[2] = 5;
            CoMassData [0]="0";
            CoMassData[1] = "1";
            CoMassData[2] = "";

            InitializeComponent();
            serchCom();
            

            aTimer = new System.Timers.Timer();
            aTimer.Interval = 1000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            // aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;

        }
        

        //методо тамера что обновляет мне экран
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                LabelData.Content = DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString();
                progressBar.Value++;
                if (progressBar.Value == 59)
                {
                    progressBar.Value = 1;
                    if (chekCam == 1)
                    {
                       // autoSendZaprocSocet();
                        Barpush();
                    }
                    if (AvtDataCom == 1)
                    {
                        autoSendZaprocSocet();
                       
                    }
                }
               
                //aTimer.Enabled = false;
            });

            //Console.WriteLine(DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString());
            // 
        }

        
        private void Barpush()
        {
            //Uri uri = new Uri("Resources\ai_back.jpg", UriKind.Relative);
            //ImageSource imgSource = new BitmapImage(uri);
            //var bitmap = new BitmapImage();
            BitmapImage bitmap = new BitmapImage();
            var stream = File.OpenRead(@"D:\wamp2\www\3\images\cam\web1.jpg");
           bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
           
            bitmap.StreamSource = stream;
           bitmap.EndInit();
           stream.Close();
            /*

             bitmap.BeginInit();
            bitmap.UriSource = (new Uri(@"E:\ai_back.jpg", UriKind.Absolute));
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            
            //D:\\wamp2\\www\\3\\images\\cam\\web1.jpg
            // E:\\1\\Project\\C#\\1\\Sever_home_v1\\Sever_home_v1\\Resources\\ai_back.JPG

            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.EndInit();
            */
            //bitmap = new BitmapImage(uri);
            //  imgCam.Source = bitmap;
            Application.Current.Dispatcher.Invoke(() =>
            {
                imgCam.Source = bitmap;
            });
           
            // bitmap = null;
            // stream = null;
            //imgCam.Source = new BitmapImage(new Uri(@"file:///E:/1/Project/C#/1/Sever_home_v1/Sever_home_v1/Resources/ai_back.JPG"));
            // BitmapFrame.Create(new Uri("Resources\ai_back.jpg)
        }

        public Socket listenSocket;

       public IPEndPoint clientep;
       public static string datasoketToGraf = "";
        private void serverCoket()
        {
            // создаем сокет
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
               listenSocket.Bind(ipPoint);


                // начинаем прослушивание
                listenSocket.Listen(5);
                //TextBox.Text = ("Сервер запущен. Ожидание подключений...");
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    handler = listenSocket.Accept();
                   
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();

                    

                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    //------------------Вот этот метод позовляет изменять Юи в главном потоке из дочерхних потоков
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        CoketShow_old.Text = CoketShow.Text;
                        CoketShow.Text = (DateTime.Now.ToLongTimeString()) + Environment.NewLine + "Сокет: " + data; //+ Environment.NewLine; = Это перенос текста на новую строку

                        //aTimer.Enabled = false;
                    });
                    //-------------Вот этот метод позовляет изменять Юи в главном потоке из дочерхних потоков

                    

                    clientep = (IPEndPoint)handler.RemoteEndPoint;
                    string strConnect = "Connected with " + clientep.Address;


                    datasoketToGraf = data;
                    RazborDateSoc();
                    data = "";
                    /* MessageBox.Show("Вы получили сообщение по сокету."+ data,
                "Консоль сокета",
    MessageBoxButton.YesNo, MessageBoxImage.Question);*/
                    // отправляем ответ
                    OtvetSocket();

                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    // break;
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Ошибка класса сокет серве." + ex.Message,
              // "Консоль сокета");
                Console.WriteLine(ex.Message);
                // тут будет сохранять паразитный запрос
               string ErroSocet = @"D:\SomeDir\ErroSocet.txt";
                using (StreamWriter sw2 = new StreamWriter(ErroSocet, true, System.Text.Encoding.Default))
                {
                    sw2.WriteLine((DateTime.Now.ToLongTimeString())+" ="+ ex.Message + " = " + data + " хакер сидит тут :" + clientep.Address);
                }
                data = "";
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                listenSocket.Close();
                serverCoket();
            }
        }




        private void serchCom()
        {
            // string[] ports = SerialPort.GetPortNames();
            for (int i = 0; i < ports.Length; i++)
            {
                Console.WriteLine("[" + i.ToString() + "] " + ports[i].ToString());
                TextBox.Text = ("[" + i.ToString() + "] " + ports[i].ToString());
            }
            if (ports.Length < 2)
            {
                btCom1.Visibility = Visibility.Hidden;
                btCom1_Copy.Visibility = Visibility.Hidden;
            }
            //LabelData.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");
        }


        private void staCom() // Метод для соедниения с сом-портом и Ардуино
        {
            //MessageBox.


            if (statusPort == 0)
            {

                // string[] ports = SerialPort.GetPortNames();

                string joined = string.Join(",", ports);
                TextBox.Text = ("порты " + joined);
                try
                {
                    // настройки порта
                    port.PortName = ports[num];
                    port.BaudRate = 115200;
                    port.DataBits = 8;
                    port.Parity = System.IO.Ports.Parity.None;
                    port.StopBits = System.IO.Ports.StopBits.One;
                    port.ReadTimeout = 1000;
                    port.WriteTimeout = 1000;
                    port.Open();
                    TextBox.Text = "открыли порт";
                    statusPort = 1;
                    Console.WriteLine("октрыли порт");




                    BrStatus.Fill = (System.Windows.Media.Brush)this.TryFindResource("StOn");


                    // Brush brush = Brushes.StOn;
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: невозможно открыть порт:" + e.ToString());
                    TextBox.Text = "невозможно открыть порт";
                    return;
                }
            }
            else
            {
                //  port.Close();
                statusPort = 0;
                Console.WriteLine("Закрыли порт");
                btnOne.Content = "Ждем Соединения";
                TextBox.Text = "Закрыли порт";
                lblPortData.Content = "Ожидаем";
            }
        }

        private void btnOne_Click(object sender, RoutedEventArgs e)
        {
            //TextBox.Text = "aaaa";
            staCom();
            lblPortData.Content = "Соединение установлено";
            //btnOne.Content = "Connection";
            //this.Cursor = null;
            // if (!currentPort.IsOpen) return;
            //currentPort.Write("1");
        }

        private void btnZero_Click(object sender, RoutedEventArgs e)
        {
            Thread myThread = new Thread(new ThreadStart(serverCoket));
            myThread.Start(); // запускаем поток
            SocetStatus.Fill = (System.Windows.Media.Brush)this.TryFindResource("StOn");
            //serverCoket();
        }

        private void ButPortClose(object sender, RoutedEventArgs e)
        {
            port.Close();
            statusPort = 0;
            Console.WriteLine("Закрыли порт");
            lblPortData.Content = "Соединение закрыто";
            TextBox.Text = "Закрыли порт";
            lblPortData.Content = "Ожидаем";
            BrStatus.Fill = (System.Windows.Media.Brush)this.TryFindResource("StatusOff");

        }
        public static void SendtoCom(string Csend)
        {
           // MessageBox.Show("Send to Arduino " + Csend, "Внимание");
            try
            {
                port.Write(Csend); 
            }
            catch (Exception e)
            {
                //  Console.WriteLine("Error: " + e.Message); Console.ReadLine(); return; 
            //    MessageBox.Show("Error in  +" + e.Message,
              //                      "вот оно че");
            }
           
        }

        private void BtSendOne_1(object sender, RoutedEventArgs e)
        {
            SendtoCom("2");
            //port.Write("2");
            Console.WriteLine("Отправили 2");

        }
        

        private void BtSendTwo_2(object sender, RoutedEventArgs e)
        {
            //port.Write("3");
            //Console.WriteLine("Отправили 3");
            LoadDateCom();
        }

        private void BtSendSix_2(object sender, RoutedEventArgs e)
        {
            SendtoCom("1");
            // port.Write("1");
            System.Threading.Thread.Sleep(1000);
            returnMessage = port.ReadLine();

            TextBox.Text = returnMessage;
            Console.WriteLine("Отправили 1");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            num = 0;
            btCom0.Visibility = Visibility.Hidden;
            btCom1.Visibility = Visibility.Hidden;
            btCom1_Copy.Visibility = Visibility.Hidden;
            TextBox.Text = ("Установлено: " + ports[0]);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            num = 1;
            btCom0.Visibility = Visibility.Hidden;
            btCom1.Visibility = Visibility.Hidden;
            btCom1_Copy.Visibility = Visibility.Hidden;
            TextBox.Text = ("Установлено: " + ports[1]);

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            num = 2;
            btCom0.Visibility = Visibility.Hidden;
            btCom1.Visibility = Visibility.Hidden;
            btCom1_Copy.Visibility = Visibility.Hidden;
            TextBox.Text = ("Установлено: " + ports[2]);

        }

        private void indiprogres(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlock1.Text = progressBar.Value.ToString();
        }

        private void ClozeWindows(object sender, EventArgs e)
        {

            /* MessageBox.Show("Закрываем прилоежние.",
                 "Внимание",
     MessageBoxButton.YesNo, MessageBoxImage.Question);*/

            System.Environment.Exit(1);
        }

        private void SendIn_Click(object sender, RoutedEventArgs e)
        {
            String SendArduino = "";
            SendArduino = InputText.Text;
         //   MessageBox.Show("Инпут текст равен :" + SendArduino, "Внимание");
            SendtoCom(SendArduino);
            //port.Write(SendArduino);
            System.Threading.Thread.Sleep(1000);
           // if (port.ReadLine() != null)
           // {
                //   string returnMessage = port.ReadLine();

                // TextBox.Text = returnMessage;
           //}

        }

        

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
            chekCam = 1;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
           
            chekCam = 0;
        }

        private void LoadDateCom()
        {
            SendtoCom("1");
            //port.Write("1");
            System.Threading.Thread.Sleep(1000);
            string returnMessage = port.ReadLine();
            TextBox.Text = returnMessage;
            //returnMessage = "12;13;14;15;16;";
            splitData = returnMessage.Split(';');

            //TextBox.Text = splitData;
            Console.WriteLine("было :" + returnMessage);
            Console.WriteLine("Получили :"+ splitData[1]);
            RazborDateCom();
        }

        //функция что разбирает данные полученные по ком-порту. Она их сохраняет
        public void RazborDateCom()
        {
           // MessageBox.Show("Запустили разбор" ,
           //       "Внимание");
            string[] writePath = new string[10];

            writePath[0] = @"D:\SomeDir\data_1.txt";
            writePath[1] = @"D:\SomeDir\data_2.txt";
            writePath[2] = @"D:\SomeDir\data_3.txt";
            writePath[3] = @"D:\SomeDir\data_4.txt";
            writePath[4] = @"D:\SomeDir\data_5.txt";
            writePath[5] = @"D:\SomeDir\data_6.txt";
            writePath[6] = @"D:\SomeDir\data_7.txt";
            writePath[7] = @"D:\SomeDir\data_8.txt";
            writePath[8] = @"D:\SomeDir\data_9.txt";
            writePath[9] = @"D:\SomeDir\data_time.txt";
            if (splitData[0]=="1")
            {
                for (int i=1; i < splitData.Length;i++)
                {
                    

                    // E:\\1\\Project\\C#\\1\\Sever_home_v1\\Sever_home_v1\\Resources\\ai_back.JPG


                    using (StreamWriter sw = new StreamWriter(writePath[i], true, System.Text.Encoding.Default))
                   {
                        sw.WriteLine(splitData[i]+"+;");
                        //MessageBox.Show("сохраняем данные: " + i,
                    //"Внимание");
                    }

                }
                using (StreamWriter sw = new StreamWriter(writePath[9], true, System.Text.Encoding.Default))
                {
                    sw.WriteLine((DateTime.Now.ToLongTimeString()) + ";");
                   
                }
            }
            
        }

        // функция для разбора данных что пришли по сокету
        public void RazborDateSoc()
        {
            //string tesOtvet = "20;12;13;14;15;16";
            data1 = data.ToString();
            if (data1.Length < 4)
            {
                // MessageBox.Show("Соекет отдал малую команду, будем передавать в ком", "Внимание");
                OtvetSocketInt = 1;
                SocToCom();
                

            }
            else
            {
                OtvetSocketInt = 0;
                splitDataSoc = data1.Split(';');
                // splitDataSoc = tesOtvet.Split(';');
                int x = Int32.Parse(splitDataSoc[0]);

                //string[] writePath2 = new string[5];
                // MessageBox.Show("Соекет отдал +"+ x,
                //       "Внимание",
                // MessageBoxButton.YesNo, MessageBoxImage.Question);
                string[] writePath2 = new string[9];
                writePath2[1] = @"D:\SomeDir\data_soc_1.txt";
                writePath2[2] = @"D:\SomeDir\data_soc_2.txt";
                writePath2[3] = @"D:\SomeDir\data_soc_3.txt";
                writePath2[4] = @"D:\SomeDir\data_soc_4.txt";
                writePath2[5] = @"D:\SomeDir\data_soc_5.txt";
                writePath2[6] = @"D:\SomeDir\data_soc_6.txt";
                writePath2[7] = @"D:\SomeDir\data_soc_7.txt";
                writePath2[8] = @"D:\SomeDir\data_soc_8.txt";
               // MessageBox.Show("солар готовиться к : " + splitDataSoc[6],
               //    "Внимание");
                
                string solarSplit = splitDataSoc[6];
               double solar = 0.0;
                //MessageBox.Show("мы получили минусовой вольтаж " + splitDataSoc[6],
                //                       "вот оно че");
                try
               {
                   solar = Convert.ToDouble(solarSplit.Replace(".", ",")); // Convert.ToDouble(solarSplit);
               }
               catch (Exception e)
               {
                   //  Console.WriteLine("Error: " + e.Message); Console.ReadLine(); return; 
                   MessageBox.Show("солар готовиться к +" + e.Message,
                                       "вот оно че");
               }

               //float solar = float.Parse(splitDataSoc[6]);
               Application.Current.Dispatcher.Invoke(() =>
           {

              solarProgress.Value = solar;
               textSolar.Text = splitDataSoc[6];
              solarMassBase();
               

           });
                /* .... */

                //MessageBox.Show("солар отдал +" + solar,
                //  "Внимание");



                if (x == 20 || x == 1 )
               {
                   for (int i = 1; i < splitDataSoc.Length; i++)
                   {
                       using (StreamWriter sw2 = new StreamWriter(writePath2[i], true, System.Text.Encoding.Default))
                       {
                           sw2.WriteLine(splitDataSoc[i] + ";");
                       }
                   }

               }
               
            }
        }

        //функция что собираетс данные с сокета для рисования графика. и пердает их на вторую форму через общий арай массив
        private void solarMassBase()
        {
            //MessageBox.Show("Пробуем собрать массив для графика солара ", "Внимание");

            if (countsolarMassive != 10)

            {
                // MessageBox.Show("Массив не меньше 10 " + splitDataSoc[6], "Внимание");

                //double sendder = double.Parse(splitDataSoc[6]);
                String datagraf = DateTime.Now.ToLongTimeString();
                double sendder =  Convert.ToDouble(splitDataSoc[6].Replace(".", ","));
                // MessageBox.Show(" Записали данные в массив " + sendder, "Внимание");
                /*
                if (sendder < 0)
                {
                    MessageBox.Show("Если пришло малое значение пишем его в 0 " + sendder, "Внимание");
                    return;

                    sendder = countsolarMassive;
                  
                }
                */
                //int asrs = (int)sendder;
                for (int n = (CoMass.Length-1); n > 0; n--)
                {
                    CoMass[n] = CoMass[(n-1)];
                    CoMassData[n] = CoMassData[(n-1)];
                }
                CoMass[0] = sendder;
                CoMassData[0] = datagraf;
                //  MessageBox.Show("Закидываем полученную цифру в соответсубщих масив-место " + asrs, "Внимание");
                countsolarMassive++;
            }  
            else {
                countsolarMassive = 0; }
        }


        private void SolarProgress(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
          // textSolar.Text = solarProgress.Value.ToString();
        }

        //функция что разбирает коды полученные по сокету
        private void SocToCom()
        {
           // MessageBox.Show("Мы запустили отправку"+data1, "Внимание");
            switch (data1)
            {
                case "12":
                    SendtoCom("2");
                   // MessageBox.Show("ушло в сокет по команде 2" , "Внимание");
                    break;
                case "13":
                    SendtoCom("3");
                    //MessageBox.Show("ушло в сокет по команде 3", "Внимание");
                    break;
                case "14":
                   // byte[] msg = Encoding.UTF8.GetBytes(reply);
                    byte[] sendbytToSocket= Encoding.UTF8.GetBytes("1;95.00;22.00;0;561;0;24.00;19.00");
                    handler.Send(sendbytToSocket);
                    //handler.S
                    //SendtoCom("3");
                   // MessageBox.Show("ушло в сокет по команде 14" + sendbytToSocket, "Внимание");
                    break;
                case "15":
                    {
                        //MessageBox.Show("ушло в сокет по команде 15", "Внимание");
                       
                          Console.WriteLine("готовимся отправить картинку");
                          // There is a text file test.txt located in the root directory.
                          string fileName = @"D:\wamp2\www\3\images\cam\web1.jpg";

                          // Send file fileName to remote device
                          Console.WriteLine("Sending {0} to the host.", fileName);

                       // bitmap = new BitmapImage(uri);
                        Bitmap buf = new Bitmap(fileName);
                        Console.WriteLine("загрузили битмап");
                        //imgCam.Source = bitmap;
                        Console.WriteLine("сделали какой-то сорс битмап");
                       // ImageConverter _imageConverter = new ImageConverter();
                       // byte[] xByte = (byte[])_imageConverter.ConvertTo(bitmap, typeof(byte[]));
                        ImageConverter converter = new ImageConverter();
                        byte[] byteArray = (byte[])converter.ConvertTo(buf, typeof(byte[]));
                        int sirz = byteArray.Length;
                        string reply = sirz.ToString();

                        Console.WriteLine("вроде как сделали массив"+ reply);
                        byte[] sendSize = Encoding.UTF8.GetBytes(reply);
                        handler.Send (sendSize);
                       System.Threading.Thread.Sleep(100);
                        handler.Send(byteArray);

                       // listenSocket.Send(byteArray);
                        Console.WriteLine("отправили картинку ?" + byteArray.Length);
                        //handler.Shutdown(SocketShutdown.Both);
                        //handler.Close();
                        break;
                    }

                  
                case "24":
                    SendtoCom("4");
                    break;

                case "25":
                    SendtoCom("5");
                    break;

                case "26":
                    SendtoCom("6");
                    //MessageBox.Show("ушло в сокет по команде 6", "Внимание");
                    break;
                case "27":
                    SendtoCom("7");
                    //MessageBox.Show("ушло в сокет по команде 7 (Radio)", "Внимание");
                    break;
                case "28":
                    SendtoCom("8");
                    //MessageBox.Show("ушло в сокет по команде 8", "Внимание");
                    break;
                case "32":
                    SendtoCom("LedOn");
                   // MessageBox.Show("ушло в сокет по команде Led on", "Внимание");
                    break;
                case "33":
                    SendtoCom("LedOff");
                   // MessageBox.Show("ушло в сокет по команде Led off", "Внимание");
                    break;
                default:
                    Console.WriteLine("Вы нажали неизвестную букву");
                    break;
            }
           ;
        }
        // функция, что запускается при обработке входящих данных из сокета. Если есть 1 (код запроса на данные) = шлем в сокет ответку 
        private void OtvetSocket()
        { if (OtvetSocketInt == 1)
            { //string reply = "Спасибо за запрос в " + data.Length.ToString()
              //            + " символов";
              //byte[] msg = Encoding.UTF8.GetBytes(reply);
              // handler.Send(msg);


                try {
                    byte[] sendbytToSocket = Encoding.UTF8.GetBytes("1;95.00;22.00;0;561;0;24.00;19.00");
                    handler.Send(sendbytToSocket);
                    string PrintTextBoxOtvetSocek = null;
                    PrintTextBoxOtvetSocek += Encoding.UTF8.GetString(sendbytToSocket);
                    Dispatcher.Invoke(() => TextBox.Text = ("Отправили на сокет ответ: " + PrintTextBoxOtvetSocek));

                }
                catch (Exception e)
                {
                    //  Console.WriteLine("Error: " + e.Message); Console.ReadLine(); return; 
                  //  MessageBox.Show("Ответ в сокет сошибкой +" + e.Message,
                    //                      "вот оно че");
                    //отключил отлов ошибки с выводом на экран, крашится при запросе с мобилы
            
                }
            }
        }
        // метод отрабаывает когда чекбокс автообновления включен
        private void CheckBox_Checked1(object sender, RoutedEventArgs e)
        {
            AvtDataCom = 1;
        }
        // метод отрабаывает когда чекбокс автообновления выключен
        private void CheckBox_Unchecked1(object sender, RoutedEventArgs e)
        {
            AvtDataCom = 0;
        }

        //метод что проверяет если чекбокс автообновление данных включен, генерирует запрос в компорт
        public void autoSendZaprocSocet()
        {
            if (AvtDataCom ==1)
            {
                TextBox.Text = "Запрашиваем данные";
                try
                {
                    SendtoCom("1");
                    // port.Write("1");
                     System.Threading.Thread.Sleep(1000);
                    returnMessage = port.ReadLine();

                    TextBox.Text = ("Ардуино отправил данные в "+ DateTime.Now.ToLongTimeString()+" "+ returnMessage);
                    Console.WriteLine("Отправили 1");
                    LoadDateCom();
                }
                catch (Exception e)
                {
                    //  Console.WriteLine("Error: " + e.Message); Console.ReadLine(); return; 
                    MessageBox.Show("Автосэндер данных из компопрта выдал ошибку: " + e.Message,
                                          "вот оно че");
                }
            }
        }

        // кнопка для открытия формы с графиком солнечной панели
        private void ShowBar(object sender, RoutedEventArgs e) 
        {
            Bar1 barTabSwhov = new Bar1();
            barTabSwhov.Owner = this;
            barTabSwhov.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

            double Top = this.Top;
            double Left = this.Left;
            barTabSwhov.Left= this.Left + (this.Height+150);
            barTabSwhov.Top = this.Top;
            barTabSwhov.Show();
        }

        int wingraptableEnable = 0;  // кнопка для открытия формы с управлением домом :)
        private void ShowWGTab(object sender, RoutedEventArgs e)
        {
           
            /*
            wingraptab.Close();
            if (wingraptableEnable == 1)
            {
                wingraptab.Close();
                wingraptab.Close();
                wingraptableEnable = 0;
            }
            */

            string win = "";
            foreach (Window window in Application.Current.Windows)
            {

                win += window.ToString();

                if (window.Title == "Умный дом")
                {
                    window.Close();
                    //wingraptab.Close();
                    wingraptableEnable = 1;
                   
                 //  MessageBox.Show(win);
                     break;
                };
                
            }
            if (wingraptableEnable ==0)
            { 
            WinGraphTab wingraptab = new WinGraphTab();
            wingraptab.Owner = this;
            wingraptab.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

                double Top = this.Top;
                double Left = this.Left;
                wingraptab.Left = this.Left;
                wingraptab.Top = this.Top - (this.Width - 250);
             wingraptab.Show();

                //MessageBox.Show(wingraptableEnable.ToString());

                 } else {wingraptableEnable = 0;}


            }
        int graptableEnable = 0;  // кнопка для открытия формы графика :)
        private void ShowGraf(object sender, RoutedEventArgs e)
        {
            string labelWindows = "";
            foreach (Window windows in App.Current.Windows)
            {
                labelWindows += windows.ToString();
                if (windows.Title == "График")
                {
                    windows.Close();
                    graptableEnable = 1;
                    break;
                };
            }
            if (graptableEnable ==0)
            {
                FromGraf fromgraf = new FromGraf();
                fromgraf.Owner = this;
                fromgraf.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
                double Top = this.Top;
                double Left = this.Left;

                fromgraf.Left = this.Left -520;
                fromgraf.Top = Top;
                fromgraf.Show();
            }
            else { graptableEnable = 0; }
        }
    }
       
}
