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
    /// Interaction logic for conform.xaml
    /// </summary>
    /// 
    
    public partial class conform : Page
    {
        private Frame mainframe;
        int status;
        int totalprice = 0;
       
        public conform(Frame mainframe,String[] choosed,int[] price,int status,int total)
        {
            InitializeComponent();
            this.mainframe = mainframe;
        }

        private void conformbackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.mainframe.Navigate(new breakfast(mainframe));
        }

        


    }
}
