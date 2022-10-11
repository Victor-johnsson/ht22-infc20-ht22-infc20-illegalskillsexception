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
            enclosureTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;
          
        }

        private void enclosureTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void OnAddButton(object sender, RoutedEventArgs e)
        {

            var name = nameTextBox.Text;
            var location = locationTextBox.Text;
            var size = sizeUpDownControl.Value;
            var capacity = myUpDownControl.Value;

            try
            {
                if (string.IsNullOrEmpty(name.Trim()) || string.IsNullOrEmpty(location.Trim()) || string.IsNullOrEmpty(sizeUpDownControl.Value.ToString()) || string.IsNullOrEmpty(myUpDownControl.Value.ToString()))
                {
                   //  MessageBox.Show("Please add info");
                   
                }

                else
                {
                    var paramNames = new string[] { "@name", "@location", "@size", "@maxAmountAnimals" };
                    var values = new object[] { name, location, size, capacity };

                    dal.CallProcedureWithParameters(paramNames, values, "usp_CreateEnclosure");

                    enclosureTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;
                        
                }
       
            }
            catch (SqlException ex)
            { 
                if (ex.Number == 2627)
                {
                    MessageBox.Show("Id already excists");
                }

                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }

            nameTextBox.Clear();
            locationTextBox.Clear();
            sizeUpDownControl.Value = 0;
            myUpDownControl.Value = 0;

        }

        private void OnDeleteButton(object sender, RoutedEventArgs e)
        {
            var selectItem = enclosureTable.SelectedItem as DataRowView;        

            if(selectItem != null)
            {
                var id = selectItem[0];

                dal.CallProcedureWithParameters(new[] { "@id" }, new[] { id }, "usp_DeleteEnclosure");
                enclosureTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadEnclosure").DefaultView;
            }
          
        }

        private void OnUpdate(object sender, DataGridRowEditEndingEventArgs e)
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

      
    }
}
