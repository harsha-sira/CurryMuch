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
      //  SerialPort serialPort1 = new SerialPort("COM18", 9600, Parity.None, 8, StopBits.One);
        String[] btnNames = { "Rice", "තෝසේ", "Noodles", "VEG 1", "VEG 2", "Fish", "Egg", "Chicken", "Fish", "Egg", "Chicken", "Coke", "Ginger beer", "Fruit salad", "Fruit juice" };
        int[] priceItem = { 50, 30, 30, 30, 30, 40, 50, 60, 40, 50, 60, 40, 40, 70, 70 };
        int totalprice =0;

        Boolean[] buttonArrayClick = new Boolean[16];
        int[] numberOfItemsChoosed = new int[16];
        ImageBrush brush = new ImageBrush();
        ImageBrush brush1 = new ImageBrush();
        ImageBrush brush2 = new ImageBrush();
        ImageBrush brush3 = new ImageBrush();

        BitmapImage bitmap = new BitmapImage();
        BitmapImage bitmap1 = new BitmapImage();
        BitmapImage bitmap3 = new BitmapImage();
        BitmapImage bitmap4 = new BitmapImage();

        bool addSelected = false;
        bool minusSelected = false;
        Boolean[] moreItemSelected = new Boolean[16];

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
            Style style = this.FindResource("MyButtonStyle") as Style;
            dinnerNextbtn.Style = style;
            addBtn.Style = style;
            minusBtn.Style = style;
            backBtn.Style = style;
            createBitmaps();

            for (int i = 0; i < 15; i++)
            {
                System.Windows.Controls.Button newBtn = new Button();
                System.Windows.Controls.Button newBtn1 = new Button();
                
                newBtn.Content = btnNames[i] + "\n" + priceItem[i];
                if (btnNames[i].Length >= 7)
                {
                    newBtn.Content = btnNames[i] + "\n    " + priceItem[i];
                }

                newBtn.FontWeight = FontWeights.Bold;
                newBtn.FontFamily = new FontFamily("Arial Black");

                // 
                BitmapImage bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.UriSource = new Uri("..//..//Resources/ButtonBackground.png", UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                brush.ImageSource = bitmap;
                newBtn.Background = brush;

                newBtn.Name = "Button" + i.ToString();
                newBtn.FontSize = 24;
                newBtn.FontStyle = FontStyles.Normal;
                newBtn.Width = 200;
                newBtn.Tag = i;
                newBtn.Height = 100;
                newBtn.Style = style;
                // Margin = new Thickness(50);
                newBtn.Margin = new Thickness(0, 20, 0, 0);
                newBtn.Click += new RoutedEventHandler(newBtn_Click);

                if (i < 3)
                {
                    panel1.Children.Add(newBtn);
                }
                else if (i < 5)
                {
                    panel2.Children.Add(newBtn);
                }
                else if (i < 8)
                {
                    panel3.Children.Add(newBtn);

                }
                else if (i < 11)
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
            string s = btn.Content.ToString();

            ImageBrush brush1 = new ImageBrush();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("..//..//Resources/tick.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            brush1.ImageSource = bitmap;

            if (!addSelected && !minusSelected)
            {
                if (!buttonArrayClick[i] && !moreItemSelected[i])
                {
                    buttonArrayClick[i] = true;
                    totalprice += priceItem[i];
                    numberOfItemsChoosed[i]++;
                    moreItemSelected[i] = true;
                    btn.Background = brush1;
                }
                else
                {
                    buttonArrayClick[i] = false;
                    totalprice -= priceItem[i];
                    btn.Background = brush;
                    numberOfItemsChoosed[i] = 0;
                    moreItemSelected[i] = false;
                    string tem = btn.Content.ToString(); //tem is temporary variable
                    if (tem.Contains("("))
                    {
                        btn.Content = tem.Substring(0, tem.IndexOf("("));
                    }
                }

            }
            else if (addSelected)
            {
                if (numberOfItemsChoosed[i] > 0)
                {
                    // if (numberOfItemsChoosed[i] != 0) {
                    numberOfItemsChoosed[i]++;
                    totalprice += priceItem[i];

                    if (s.Contains("("))
                    {
                        int index = s.IndexOf("(");
                        btn.Content = s.Substring(0, index) + "(" + numberOfItemsChoosed[i] + ")";
                    }
                    else
                    {
                        btn.Content += "(" + numberOfItemsChoosed[i] + ")";
                    }

                    //  }
                }
            }
            else if (minusSelected)
            {
                if (numberOfItemsChoosed[i] > 1)
                {
                    // if (numberOfItemsChoosed[i] != 0) {
                    numberOfItemsChoosed[i]--;
                    totalprice -= priceItem[i];

                    if (s.Contains("("))
                    {
                        int index = s.IndexOf("(");
                        btn.Content = s.Substring(0, index) + "(" + numberOfItemsChoosed[i] + ")";
                    }
                    else
                    {
                        btn.Content += "(" + numberOfItemsChoosed[i] + ")";
                    }

                    //  }
                }
                else if (numberOfItemsChoosed[i] == 1)
                {
                    try
                    {
                        btn.Content = btn.Content.ToString().Substring(0, btn.Content.ToString().IndexOf("("));
                    }
                    catch (Exception ex)
                    {

                    }
                    totalprice -= priceItem[i];
                    numberOfItemsChoosed[i] = 0;
                    buttonArrayClick[i] = false;
                    btn.Background = brush;
                }

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

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            minusSelected = false;
            brush3.ImageSource = bitmap3;
            minusBtn.Background = brush3;

            brush2.ImageSource = bitmap1;
            addBtn.Background = brush2;

            addSelected = !addSelected;
            if (addSelected)
            {
                brush2.ImageSource = bitmap;
                addBtn.Background = brush2;
            }
            else
            {
                addBtn.Background = brush2;

            }
        }

        private void minusBtn_Click(object sender, RoutedEventArgs e)
        {
            //BitmapImage bitmap3 = new BitmapImage();
            //BitmapImage bitmap4 = new BitmapImage();

            addSelected = false;
            brush2.ImageSource = bitmap1;
            addBtn.Background = brush2;
           
            brush3.ImageSource = bitmap3;
            minusBtn.Background = brush3;

            minusSelected = !minusSelected;
            if (minusSelected)
            {

                brush3.ImageSource = bitmap4;
                minusBtn.Background = brush3;
            }
            else
            {
                brush3.ImageSource = bitmap3;
                minusBtn.Background = brush3;

            }

        }

        public void createBitmaps()
        {

            bitmap1.BeginInit();
            bitmap1.UriSource = new Uri("..//..//Resources/plus-button.png", UriKind.RelativeOrAbsolute);
            bitmap1.EndInit();
            
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("..//..//Resources/plus-button-active.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            
            bitmap3.BeginInit();
            bitmap3.UriSource = new Uri("..//..//Resources/minus-button.png", UriKind.RelativeOrAbsolute);
            bitmap3.EndInit();
           
            bitmap4.BeginInit();
            bitmap4.UriSource = new Uri("..//..//Resources/minus-button-active.png", UriKind.RelativeOrAbsolute);
            bitmap4.EndInit();
          
        }
    }
}
