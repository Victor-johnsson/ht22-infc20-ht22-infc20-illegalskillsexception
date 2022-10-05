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
    /// Interaction logic for Enclosure.xaml
    /// </summary>
    public partial class Enclosure : Page
    { 
        private DataAccessLayer dal = new DataAccessLayer();
        public Enclosure()
        {
            InitializeComponent();
            enclosureTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;
             // animalComboBox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;
            //<ComboBox x:Name="animalComboBox" DisplayMemberPath="Name" SelectedValuePath="Id" Canvas.Left="490" Canvas.Top="72" Width="102" />
        }

        private void enclosureTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void onAddButton(object sender, RoutedEventArgs e)
        {

            var name = nameTextBox.Text;
            var location = locationTextBox.Text;
            var size = sizeTextBox.Text;
            var capacity = myUpDownControl.Text;

            var paramNames = new string[] { "@name", "@location" ,"@size" , "@maxAmountAnimals"};
            var values = new object[] { name, location, size, capacity };

            dal.CallProcedureWithParameters(paramNames, values, "usp_CreateEnclosure");


            enclosureTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;
        }

        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
