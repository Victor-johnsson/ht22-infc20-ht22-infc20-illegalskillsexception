using Microsoft.Extensions.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using System.Diagnostics;

namespace ProjAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        
        public MainWindow()
        {

            DataAccessLayer dal = new DataAccessLayer();
            dal.TryConnection();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Debug.WriteLine("hello");
            textBox.Text = "Hello";
            
        }
    }
}
