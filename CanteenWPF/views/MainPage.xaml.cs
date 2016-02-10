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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Frame mainframe;
        
        public MainPage(Frame mainframe)
        {
            InitializeComponent();
            Style style = this.FindResource("MyButtonStyle") as Style;
            this.mainframe = mainframe;
            breakfastBtn.Style = style;
            lunchBtn.Style = style;
            dinnerBtn.Style = style;

        }

        private void breakfastBtn_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Navigate(new breakfast(mainframe));
        }

        private void lunchBtn_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Navigate(new lunch(mainframe));
        }

        private void dinnerBtn_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Navigate(new dinner(mainframe));
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
