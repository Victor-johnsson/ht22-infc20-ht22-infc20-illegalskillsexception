using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
            
            foodRadioButton.IsChecked = true;
            allAnimalsRadioButton.IsChecked = true;

            FillComboboxes();
        }



        private void FillComboboxes()
        {
            try
            {
                animalNameCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
                enclosureComboBox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;
                foodComboBox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadFood").DefaultView;
                if (foodRadioButton.IsChecked.Value)
                {
                    changeCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadFood").DefaultView;


                }
                else
                {
                    changeCombobox.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;

                }
            }
            catch(SqlException ex)
            {
                errorMsg.Content = ErrorHandling.SqlError(ex);
            }


           

            
        }
        private void FillTable()
        {
            try
            {
                if (allAnimalsRadioButton.IsChecked == true)
                {
                    animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;

                }
                else
                {
                    animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadNewAnimal").DefaultView;

                }
            }
            catch(SqlException ex)
            {
                errorMsg.Content = ErrorHandling.SqlError(ex);
            }
            
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            try
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
                   && food != null
                   && enclosure != null
                   && foodAmount != null
                   && foodAmount >= 100
                   && foodAmount <= 10000
                   )

                {

                    dal.CallProcedureWithParameters(
                   new[] { "@name", "@foodId", "@enclosureId", "@type", "@foodAmount" },
                   new object[] { name, food, enclosure, type, foodAmount },
                   "usp_CreateAnimal");
                    FillTable();
                    
                    FillComboboxes();

                    animalNameTextBox.Clear();
                    animalTypeTextBox.Clear();
                    foodIntegerUpDown.Value = 100;
                    errorMsg.Content = "";

                }
            } catch (SqlException ex) {
                errorMsg.Content = ErrorHandling.SqlError(ex);
                FillTable();
                FillComboboxes();

            }

        }





        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var selection = animalTable.SelectedItem as DataRowView;

                if (selection != null)
                {

                    var id = selection["Id"];
                    dal.CallProcedureWithParameters(new[] { "@id" }, new[] { id }, "usp_DeleteAnimal");

                    FillComboboxes();
                    FillTable();
                    errorMsg.Content = null;

                }
                else
                {
                    errorMsg.Content = "Please select a row to delete";
                }
            }
            catch (SqlException ex)
            {
                errorMsg.Content = ErrorHandling.SqlError(ex);

            }
           



        }

        private void UpdateAnimal(object sender, DataGridRowEditEndingEventArgs e)
        {

           
            try
            {
                var dataGridRow = e.Row;
                var row = dataGridRow.Item as DataRowView;
                if (row != null)
                {
                    var itemArray = row.Row;
                    object?[] values = { itemArray["id"], itemArray["animalName"], itemArray["foodAmount"], itemArray["enclosureId"], itemArray["foodId"], itemArray["animalType"] };


                    dal.CallProcedureWithParameters(
                       new[] { "@id", "@name", "@foodAmount", "@enclosureId", "@foodId", "@animalType" },
                       values,
                       "usp_UpdateAnimal");


                    FillTable();
                    FillComboboxes();

                }
            } catch (SqlException ex) {
                errorMsg.Content = ErrorHandling.SqlError(ex);
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
            try
            {
                var animal = animalNameCombobox.SelectedItem as DataRowView;
                var item = changeCombobox.SelectedItem as DataRowView;

                if (animal != null && item != null)
                {
                    var itemRow = item.Row;
                    var animalRow = animal.Row;
                    var values = new object[5];
                    values[0] = animalRow["Id"];
                    values[1] = animalRow["animalName"];
                    values[2] = animalRow["foodAmount"];
                    values[5] = animal["animalType"];

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

                    dal.CallProcedureWithParameters(new[] { "@id", "@name", "@foodAmount", "@enclosureId", "@foodId", "@animalType"}, values, "usp_UpdateAnimal");

                    FillTable();
                    FillComboboxes();
                    changeCombobox.SelectedItem = null;

                    errorMsg.Content = null;

                }
                else
                {
                    errorMsg.Content = "Please enter values";

                }
            } catch (SqlException ex) {
                errorMsg.Content = ErrorHandling.SqlError(ex);
                FillComboboxes();
               
            }
        } 
        


        private void OnAllAnimalRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            animalTable.Columns.Clear();


            Binding binding = new Binding("animalName");
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            DataGridTextColumn txtColumn = new DataGridTextColumn();
            txtColumn.Header = "Name";
            txtColumn.Binding = binding;
            animalTable.Columns.Add(txtColumn);


            binding = new Binding("enclosureName");
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            txtColumn = new DataGridTextColumn();
            txtColumn.Header = "Enclosure";
            txtColumn.Binding = binding;
            txtColumn.IsReadOnly= true;
            animalTable.Columns.Add(txtColumn);

            binding = new Binding("animalType");
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            txtColumn = new DataGridTextColumn();
            txtColumn.Header = "Species";
            txtColumn.Binding = binding;
            animalTable.Columns.Add(txtColumn);


            binding = new Binding("foodType");
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            txtColumn = new DataGridTextColumn();
            txtColumn.Header = "Food";
            txtColumn.Binding = binding;
            txtColumn.IsReadOnly = true;
            animalTable.Columns.Add(txtColumn);

            binding = new Binding("FoodAmount");
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            txtColumn = new DataGridTextColumn();
            txtColumn.Header = "Food Amount";
            txtColumn.Binding = binding;
            animalTable.Columns.Add(txtColumn);




            deleteButton.IsEnabled = true;
            animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadAnimal").DefaultView;
            FillTable();

        }

        private void OnNewAnimalRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            animalTable.Columns.Clear();
            
            DataGridTextColumn txtColumn = new DataGridTextColumn();
            txtColumn.Header = "Name";
            txtColumn.Binding = new Binding("animalName");
            txtColumn.IsReadOnly = true;
           
            animalTable.Columns.Add(txtColumn);
            

            DataGridTextColumn dateColumn = new DataGridTextColumn();
            dateColumn.Header = "Date";
            dateColumn.Binding = new Binding("date");
            animalTable.Columns.Add(dateColumn);
            dateColumn.Binding.StringFormat = "dd/MM-yyyy";
            dateColumn.IsReadOnly = true;


            deleteButton.IsEnabled = false;
            animalTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadNewAnimal").DefaultView;
            FillTable();
        }
    }

    
}
