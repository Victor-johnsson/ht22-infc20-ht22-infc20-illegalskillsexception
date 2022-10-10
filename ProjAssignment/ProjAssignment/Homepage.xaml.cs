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

namespace ProjAssignment
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Homepage : Window
    {
        public Homepage()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void OnCaretakerClick(object sender, RoutedEventArgs e)
        {
            contentFrame.Source = new Uri("../Caretaker.xaml", UriKind.Relative);
        }

        private void OnAnimalsClick(object sender, RoutedEventArgs e)
        {
            contentFrame.Source = new Uri("../Animal.xaml", UriKind.Relative);
        }

        private void OnEnclosureClick(object sender, RoutedEventArgs e)
        {
            contentFrame.Source = new Uri("../Enclosure.xaml", UriKind.Relative);
        }
    }
}
