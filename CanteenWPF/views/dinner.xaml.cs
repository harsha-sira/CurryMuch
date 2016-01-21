using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Windows.Shapes;

namespace CanteenWPF
{
    /// <summary>
    /// Interaction logic for dinner.xaml
    /// </summary>
    /// 
    
    public partial class dinner : Page
    {
        private Frame mainframe;
        SerialPort serialPort1 = new SerialPort("COM18", 9600, Parity.None, 8, StopBits.One);
        String[] btnNames = { "Rice", "String Hoppers", "Noodles", "Kiribath", "VEG 1", "VEG 2", "Fish", "Egg", "Chicken", "Fish", "Egg", "Chicken", "Coke", "Ginger beer", "Fruit salad", "Fruit juice" };
        int[] priceItem = { 50, 30, 30, 30, 30, 30, 40, 50, 60, 40, 50, 60, 40, 40, 70, 70 };
        int totalprice =0;
        Boolean[] buttonArrayClick = new Boolean[16];

        /*
         *clear the array  
         */
        private void clearButtonArrayClick()
        {
            for (int i = 0; i < buttonArrayClick.Length; i++)
            {
                buttonArrayClick[i] = false;
            }
        }
        public dinner(Frame mainframe)
        {
            InitializeComponent();
            this.mainframe = mainframe;
            mainframe.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            //serial port coonection
            //serialPort1.PortName = "COM18";
            //try
            //{
            //    serialPort1.Open();
            //}
            //catch
            //{
            //    Environment.Exit(0);
            //}

            //Thread mainThread = new Thread(new ThreadStart(checker));
            //mainThread.Start();

            for (int i = 0; i < 16; i++)
            {
                System.Windows.Controls.Button newBtn = new Button();
                System.Windows.Controls.Button newBtn1 = new Button();

                newBtn.Content = btnNames[i];
                newBtn.Background = null;
                newBtn.Name = "Button" + i.ToString();
                newBtn.FontSize = 24;
                newBtn.FontStyle = FontStyles.Normal;
                newBtn.Width = 200;
                newBtn.Tag = i;
                newBtn.Height = 70;
                // Margin = new Thickness(50);
                newBtn.Margin = new Thickness(0, 20, 0, 0);
                newBtn.Click += new RoutedEventHandler(newBtn_Click);

                if (i < 4)
                {
                    panel1.Children.Add(newBtn);
                }
                else if (i < 6)
                {
                    panel2.Children.Add(newBtn);
                }
                else if (i < 9)
                {
                    panel3.Children.Add(newBtn);

                }
                else if (i < 12)
                {
                    panel4.Children.Add(newBtn);
                }
                else
                {
                    panel5.Children.Add(newBtn);
                }



            }
            clearButtonArrayClick();
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int i = (int)btn.Tag;

            ImageBrush brush = new ImageBrush();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("img/tick.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            brush.ImageSource = bitmap;

            if (!buttonArrayClick[i])
            {
                buttonArrayClick[i] = true;
                totalprice += priceItem[i];
                btn.Background = brush;
            }
            else
            {
                buttonArrayClick[i] = false;
                totalprice -= priceItem[i];
                btn.Background = null;
            }

        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.mainframe.Navigate(new MainPage(mainframe));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.mainframe.Navigate(new Payment(mainframe, totalprice, 3));
        }


    }
}
