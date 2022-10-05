using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Animal.xaml
    /// </summary>
    public partial class Animal : Page
    {
        private DataAccessLayer dal = new DataAccessLayer();
        public Animal()
        {
            InitializeComponent();
            animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
            enclosureComboBox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;
            foodComboBox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadFood").DefaultView;
        }

        private void animalTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void enclosureComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            var name = animalNameTextBox.Text;
            var food = foodComboBox.SelectedValue;
            var enclosure = enclosureComboBox.SelectedValue;

            var type = animalTypeTextBox.Text;
            var foodAmount = foodIntegerUpDown.Value;

            dal.CallProcedureWithParameters(
                new[] { "@name", "@foodId", "@enclosureId", "@type", "@foodAmount" },
                new object[] { name, food, enclosure, type, foodAmount },
                "usp_CreateAnimal");
            animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;

        }

    }
}
