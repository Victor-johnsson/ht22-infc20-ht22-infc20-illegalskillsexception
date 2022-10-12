using System;
using System.Collections.Generic;
using System.Data;
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
            animalNameCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
            foodRadioButton.IsChecked = true;
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
            var name = animalNameTextBox.Text.Trim();
            var food = foodComboBox.SelectedValue;
            var enclosure = enclosureComboBox.SelectedValue;
            var type = animalTypeTextBox.Text.Trim();
            var foodAmount = foodIntegerUpDown.Value;


            if (
                !string.IsNullOrEmpty(name) 
                && name.Length >= 1
                && name.Length <= 255
                && !string.IsNullOrEmpty(type) 
                && type.Length >= 1
                && type.Length <= 255
                && food!= null
                && enclosure!= null 
                && foodAmount!=null
                && foodAmount >=100
                && foodAmount <= 10000
                ) 
            {
               
               dal.CallProcedureWithParameters(
              new[] { "@name", "@foodId", "@enclosureId", "@type", "@foodAmount" },
              new object[] { name, food, enclosure, type, foodAmount },
              "usp_CreateAnimal");
                animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
                animalNameCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;

                animalNameTextBox.Clear();
                animalTypeTextBox.Clear();
                foodIntegerUpDown.Value = 100;

            }

            else
            {
                errorMsg.Content = "Please enter all fields";

            }    

        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            var selection = animalTable.SelectedItem as DataRowView;

            if (selection != null) {

                var id = selection["Id"];
                dal.CallProcedureWithParameters(new[] { "@id" }, new[] { id }, "usp_DeleteAnimal");
               
                animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
                animalNameCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
                errorMsg.Content = null;

            }
            else
            {
                errorMsg.Content = "Please select a row to delete";
            }


              
        }

        private void UpdateAnimal(object sender, DataGridRowEditEndingEventArgs e)
        {
            var dataGridRow = e.Row;
            var row = dataGridRow.Item as DataRowView;
            if (row != null)
            {
                var itemArray = row.Row;
                object?[] values = { itemArray["id"], itemArray["animalName"], itemArray["foodAmount"], itemArray["enclosureId"], itemArray["foodId"] };


                dal.CallProcedureWithParameters(
                   new[] { "@id", "@name", "@foodAmount", "@enclosureId", "@foodId" },
                   values,
                   "usp_UpdateAnimal");


                animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
                animalNameCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;

            }
        }

        private void OnEnclosureRadioButton(object sender, RoutedEventArgs e)
        {
            changeCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;
            changeCombobox.DisplayMemberPath = "Name";
        }

        private void OnFoodRadioButton(object sender, RoutedEventArgs e)
        {
            changeCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadFood").DefaultView;
            changeCombobox.DisplayMemberPath = "Type";
        }

        private void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {

            var animal = animalNameCombobox.SelectedItem as DataRowView;
            var item = changeCombobox.SelectedItem as DataRowView;

            if (animal != null && item !=null )
            {
                var itemRow = item.Row;
                var animalRow = animal.Row;
                var values = new object[5];
                values[0] = animalRow["Id"];
                values[1] = animalRow["animalName"];
                values[2] = animalRow["foodAmount"];

                if (foodRadioButton.IsChecked.Value)
                {
                    values[4] = itemRow["Id"];
                    values[3] = animalRow["enclosureId"];

                }
                else
                {
                    values[3] = itemRow["Id"];
                    values[4] = animalRow["foodId"];
                }
                dal.CallProcedureWithParameters(new[] { "@id", "@name", "@foodAmount", "@enclosureId", "@foodId" }, values, "usp_UpdateAnimal");

                animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
                animalNameCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
                changeCombobox.SelectedItem = null;

                errorMsg.Content=null;

            }
            else
            {
                errorMsg.Content = "Please enter values";

            }


        }
    }
}
