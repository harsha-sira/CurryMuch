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
        bool auth=false, returnTotop = false;
        int status;
        int counter =0;
       static SerialPort serialPort1=null;
        
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

                            Debug.WriteLine(s);
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

            
            


            Debug.WriteLine("afterquit");
            InitializeComponent();
            this.mainframe = mainframe;
            this.total = total;
            this.status = status;

            ImageBrush brush = new ImageBrush();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("img/nfc.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            brush.ImageSource = bitmap;
            lb.Background = brush;

            priceLable.Content = total.ToString() + " RS /=";
         // outputLable.Content = "wada";
            threadCreator();
        }


        private void threadCreator()
        {
            Thread mainThread = new Thread(new ThreadStart(checker));
            mainThread.SetApartmentState(ApartmentState.STA);
            Debug.WriteLine("df");
            mainThread.Start();
        }

        //////////////////////////////
        private void checker()
        {
            String temp = "";
            bool isrunning = true;

            while (isrunning)
            {
                try
                {

                    temp = serialPort1.ReadLine();
                  // temp = "1.2.3.4.5.6.7.8.";
                    serialPort1.Close();
                    Debug.WriteLine(temp);
                }
                catch (Exception e)
                {
                    Environment.Exit(0);
                }

                if (!temp.Equals(""))
                {
                    
                    cardnumber = temp;
                    outputLable.Dispatcher.Invoke(() => outputLable.Content = "Please Wait . . . . .");
                    auth = true;
                    temp = "";
                }

                //..............................
                //if (priceLable.Content!= "0 RS /=" && usernameLable.Content != "Not Detected ")
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
                        String amount = responseString.Substring(Start, End - Start);

                        usernameLable.Dispatcher.Invoke((() => usernameLable.Content = username));
                      //  label13.Invoke((MethodInvoker)(() => label13.Text = amount));
                        
                        returnTotop = true;

                        if (Int32.Parse(amount) < 0)
                        {
                            outputLable.Dispatcher.Invoke(() => outputLable.Content = "Paid Sucessfully ! Thank you ! \n RECHARGE YOUR ACCOUNT QUICKLY !!!!" +
                                                    "\n Your remaining Balance is " +amount + " RS /=");
                        }
                        else
                        {
                            outputLable.Dispatcher.Invoke(() => outputLable.Content = "Paid Sucessfully ! Thank you ! " +
                                                     "\nYour remaining Balance is " +amount + " RS /=");
                        }


                    }
                    else
                    {
                        returnTotop = true;
                        outputLable.Dispatcher.Invoke(() => outputLable.Content = "Sorry ! INVALID CARD. ");
                        
                    }

                    auth = false;

                }
                if (returnTotop)
                {
                    
                    Thread.Sleep(5000);
                    total = 0;
                    returnTotop = false;
                   
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

                Thread.Sleep(10);

            }
        }

    }
}
