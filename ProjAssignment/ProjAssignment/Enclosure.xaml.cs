using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
            try
            {
                enclosureTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;

            }
            catch (SqlException ex)
            {

                errorMsg.Content = ErrorHandling.SqlError(ex);
            }

        }

  
        private void OnAddButton(object sender, RoutedEventArgs e)
        {

            var name = nameTextBox.Text.Trim();
            var location = locationTextBox.Text.Trim();
            var size = sizeUpDownControl.Value;
            var capacity = myUpDownControl.Value;
            

            try
            {
                if (
                    !string.IsNullOrEmpty(name) 
                    && !string.IsNullOrEmpty(location) 
                    && name.Length >= 1
                    && name.Length <= 255
                    && location.Length >=1
                    && location.Length <= 255
                    && size != null 
                    && capacity!= null   
                    && size >= 1
                    && size <= 30000
                    && capacity >=1
                    && capacity <= 1000
                    )
                {
                    var paramNames = new string[] { "@name", "@location", "@size", "@maxAmountAnimals" };
                    var values = new object[] { name, location, size, capacity };

                    dal.CallProcedureWithParameters(paramNames, values, "usp_CreateEnclosure");

                    enclosureTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;

                    nameTextBox.Clear();
                    locationTextBox.Clear();
                    sizeUpDownControl.Value = 0;
                    myUpDownControl.Value = 0;
                }

                else
                {

                    errorMsg.Content = "Please enter valid information";

                }
       
            }
            catch (SqlException ex)
            { 
                
                errorMsg.Content = ErrorHandling.SqlError(ex);
                

               
            }
        }

        private void OnDeleteButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectItem = enclosureTable.SelectedItem as DataRowView;

                if (selectItem != null)
                {
                    var id = selectItem[0];

                    dal.CallProcedureWithParameters(new[] { "@id" }, new[] { id }, "usp_DeleteEnclosure");
                    enclosureTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;

                    errorMsg.Content = null;
                }

                else
                {
                    errorMsg.Content = "Please select row to delete";
                }


            }
            catch (SqlException ex)
            {
                errorMsg.Content = ErrorHandling.SqlError(ex);

            }
          
        }

        private void OnUpdate(object sender, DataGridRowEditEndingEventArgs e)
        {
            try
            {
                var dataGridRow = e.Row;
                var row = dataGridRow.Item as DataRowView;

                if (row != null)
                {
                    object?[] itemArray = row.Row.ItemArray;

                    dal.CallProcedureWithParameters(
                       new[] { "@id", "@size", "@location", "@maxAmountAnimals", "@name" },
                       itemArray,
                       "usp_UpdateEnclosure");
                    enclosureTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;

                }
            }
            catch(SqlException ex)
            {
                errorMsg.Content = ErrorHandling.SqlError(ex);

            }
           

        }

      
    }
}
