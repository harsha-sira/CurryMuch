using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CanteenWPF
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Page
    {
        private Frame mainframe;
        int total = 0;
        String cardnumber = "";
        bool returnTotop = false;
        int status;
        String amount;
        static SerialPort serialPort1=null;
        Thread mainThread;
        bool isrunning = true;
        bool closeserialport = false;
        String temp = "";
        
        public Payment(Frame mainframe, int total,int status) //staus is used to identify lunch,breakfast and dinner
        {

            if (serialPort1 == null)
            {
                foreach (string s in SerialPort.GetPortNames())
                {
                  
                    serialPort1 = new SerialPort(s, 9600, Parity.None, 8, StopBits.One);
                    if (!serialPort1.IsOpen)
                    {

                        try
                        {
                            serialPort1.Open();

                            Debug.WriteLine("open");
                            break;
                        }
                        catch
                        {
                            continue;

                        }
                    }

                }

            }


            
            //serial port coonection
         //   serialPort1.PortName = "COM18";

        //    serialPort1 = new SerialPort("COM10", 9600, Parity.None, 8, StopBits.One);
           
            


            Debug.WriteLine("afterquit");
            InitializeComponent();
            this.mainframe = mainframe;
            this.total = total;
            this.status = status;

            priceLable.Content = total.ToString() + " RS /=";
         // outputLable.Content = "wada";
            threadCreator();
        }


        private void threadCreator()
        {
            mainThread = new Thread(new ThreadStart(checker));
            mainThread.SetApartmentState(ApartmentState.STA);
            Debug.WriteLine("df");
            mainThread.Start();
        }

        //////////////////////////////
        private void checker()
        {
            Debug.WriteLine("ddf"); //to del

           
            while (isrunning)
            {
                Debug.WriteLine("running"); // to del
               
                    try
                    { 
                        temp += serialPort1.ReadExisting();
                        Debug.WriteLine(temp); //to del
                  //  temp += (char)serialPort1.ReadChar();
                        if (temp.Contains("*"))
                        {
                            serialPort1.Close();
                            serialPort1 = null;
                            Debug.WriteLine("closed serialport"); // to del
                            closeserialport = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Environment.Exit(0);
                    }
                    

                

                
                if (!temp.Equals("") && closeserialport && total != 0)
                {

                    Debug.WriteLine("length 16"); // to del
                    Debug.WriteLine("card->"+ temp);
                    cardnumber = temp;
                    cardnumber = cardnumber.Substring(0, cardnumber.Length - 1);
                    Debug.WriteLine("card"+ cardnumber); //to del
                    outputLable.Dispatcher.Invoke(() => outputLable.Content = "Please Wait . . . . .");
                    
                    temp = "";

                    if (paymentgrid.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
                    {
                        imagebrushdrawing();
                        usernameLable.Visibility = Visibility.Visible;
                        cancelBtn.Visibility = Visibility.Hidden; 
                        

                    }
                    else
                    {
                        paymentgrid.Dispatcher.BeginInvoke((System.Threading.ThreadStart)(delegate
                        {
                            imagebrushdrawing();
                            usernameLable.Visibility = Visibility.Visible;
                            cancelBtn.Visibility = Visibility.Hidden; 
                        
                        }));
                    }

                }
                else if (total == 0)
                {

                    if (paymentgrid.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
                    {
                        imagebrushdrawing();
                      //  usernameLable.Visibility = Visibility.Visible;
                        cancelBtn.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        paymentgrid.Dispatcher.BeginInvoke((System.Threading.ThreadStart)(delegate
                        {
                            imagebrushdrawing();
                        //    usernameLable.Visibility = Visibility.Visible;
                            cancelBtn.Visibility = Visibility.Hidden;
                        }));
                    }

                    outputLable.Dispatcher.Invoke(() => outputLable.Content = "No item selected");
                    returnTotop = true;
                }

              //  Debug.WriteLine(cardnumber); //to del
              // Debug.WriteLine(total); //to del

                //..............................
                if ( cardnumber!="" && total!= 0)
                {
                    Console.WriteLine("inside");
                    // send data to inernet
                    String user = cardnumber;
                    String cost = total.ToString();

                    //   var request = (HttpWebRequest)WebRequest.Create("http://localhost/vega/search.php");
                    // var request = (HttpWebRequest)WebRequest.Create("http://vega.netau.net/put.php");
                    var request = (HttpWebRequest)WebRequest.Create("http://uom.comeze.com/backend/search.php");


                    var postData = "uid=" + user;
                    postData += "&cost=" + cost;
                    var data = Encoding.ASCII.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    //should check internet connectivity
                    if (NetworkInterface.GetIsNetworkAvailable() != true)
                    {
                        outputLable.Dispatcher.Invoke(() => outputLable.Content = "No Network connection ");
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                        
                    }

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    
                    

                    var response = (HttpWebResponse)request.GetResponse();

                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    //Debug.WriteLine(responseString);

                    if (responseString.Contains("TrueItsWorking"))
                    {
                        int Start = responseString.IndexOf("TrueItsWorking", 0) + 14;
                        int End = responseString.IndexOf("###@#", Start);


                        String username = responseString.Substring(Start, End - Start);
                        Start = responseString.IndexOf("###@#", 0) + 5;
                        End = responseString.IndexOf("###@@", Start);
                        amount = responseString.Substring(Start, End - Start);

                        usernameLable.Dispatcher.Invoke((() => usernameLable.Content = username));
                      //  label13.Invoke((MethodInvoker)(() => label13.Text = amount));
                        
                        returnTotop = true;

                        if (Int32.Parse(amount) < 0)
                        {
                           outputLable.Dispatcher.Invoke(() => outputLable.Content = "PLEASE RECHARGE QUICKLY !");
                           priceLable.Dispatcher.Invoke(() => priceLable.Content = amount+ " RS/=");
                        }
                        else
                        {
                           outputLable.Dispatcher.Invoke(() => outputLable.Content = "Paid Sucessfully ! Thank you ! ");
                           priceLable.Dispatcher.Invoke(() => priceLable.Content = amount + " RS/=");
                        }


                    }
                    else if(responseString.Contains("no balance"))
                    {
                        int Start = responseString.IndexOf("no balance", 0) + 10;
                        int End = responseString.IndexOf("@@", Start);

                        outputLable.Dispatcher.Invoke(() => outputLable.Content = "You can't buy.\nPLEASE RECHARGE QUICKLY ! ");
                        
                        amount = responseString.Substring(Start,End - Start);
                        priceLable.Dispatcher.Invoke(() => priceLable.Content = amount + " RS/=");
                        returnTotop = true;
                    }
                    else
                    {
                        returnTotop = true;
                        outputLable.Dispatcher.Invoke(() => outputLable.Content = "Sorry ! INVALID CARD. ");
                        
                    }



                }
                
                if (returnTotop)
                {
                    
                    Thread.Sleep(5000);
                    total = 0;
                    returnTotop = false;


                    returnToPreviousPanel();
                   
                   

                }

                Thread.Sleep(10);
            }

            if (!isrunning)
            {
                try
                {
                    serialPort1.Close();
                    serialPort1 = null;
                }catch(Exception ex){

                }
                Debug.WriteLine("closed serialport special"); // to del
                isrunning = true;
                mainThread.Abort();
            }

        }

        void imagebrushdrawing()
        {
            BitmapImage bitmap = new BitmapImage();
            ImageBrush brush = new ImageBrush();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("..//..//Resources/backgroundConfirm.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            brush.ImageSource = bitmap;
            paymentgrid.Background = brush;
        }
        void imagebrushd()
        {
            BitmapImage bitmap = new BitmapImage();
            ImageBrush brush = new ImageBrush();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("..//..//Resources/backgroundPay.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            brush.ImageSource = bitmap;
            paymentgrid.Background = brush;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {

            isrunning = false;
            returnToPreviousPanel();
            total = 0;
            Debug.WriteLine(isrunning+"......."); // to del
            
        }

        public void returnToPreviousPanel()
        {
            if (status == 1)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Debug.WriteLine("Finish");
                    mainframe.Navigate(new breakfast(mainframe));
                    isrunning = false;

                }));


            }
            else if (status == 2)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Debug.WriteLine("Finish");
                    
                    mainframe.Navigate(new lunch(mainframe));
                    isrunning = false;
                    //mainThread.Abort();
                }));
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Debug.WriteLine("Finish");
                    mainframe.Navigate(new dinner(mainframe));
                    isrunning = false;
                }));


            }
        }

    }
}
