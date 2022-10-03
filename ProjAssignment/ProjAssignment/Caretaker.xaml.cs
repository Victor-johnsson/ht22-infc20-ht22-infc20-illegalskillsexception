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
    /// Interaction logic for Caretaker.xaml
    /// </summary>
    public partial class Caretaker : Page
    {
        private DataAccessLayer dal = new DataAccessLayer();
        public Caretaker()
        {
            
            InitializeComponent();
            caretakerTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadCaretaker").DefaultView;
        }

        private void OnCellUpdated(object sender, DataGridCellEditEndingEventArgs e)
        {

            var column = e.Column;
            var rowView = e.Row.Item as DataRowView;
            var row = rowView.Row;
            var itemArray = row.ItemArray;

            var editedColumn = e.EditingElement;
            var s = editedColumn.ToString();
            var editText = s.Split(": ")[1];
            itemArray[column.DisplayIndex + 1] = editText;

            var paramNames = new string[] { "@id", "@name", "@phoneNumber" };
            dal.CallProcedureWithParameters(paramNames, itemArray, "usp_UpdateCaretaker");

           
        }

        private void OnAddButton(object sender, RoutedEventArgs e)
        {
            var name = nameTextBox.Text;
            var phoneNumber = phoneNbrTextBox.Text;
            var paramNames = new string[] { "@name", "@phoneNbr" };
            var values = new object[] { name, phoneNumber };

            dal.CallProcedureWithParameters(paramNames, values, "usp_CreateCaretaker");
            

            caretakerTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadCaretaker").DefaultView;
        }


        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OnDeleteBtn(object sender, RoutedEventArgs e)
        {
            var rowView = caretakerTable.SelectedItem as DataRowView;
            if(rowView != null)
            {
                var row = rowView.Row;
                
                int id = (int)row.ItemArray[0];

                var paramNames = new string[] { "@id" };
                var values = new object[] { id };

                dal.CallProcedureWithParameters(paramNames, values, "usp_DeleteCaretaker");

                caretakerTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadCaretaker").DefaultView;
            }
           


        }

        private void OnRowEditEnd(object sender, DataGridRowEditEndingEventArgs e)
        {
            //caretakerTable.CommitEdit();
            //var rowView = e.Row.Item as DataRowView;
            
            
            //// e.ToString();
            //Debug.WriteLine(e.ToString());
            //if (rowView != null)
            //{
                

                //int id = (int)row.ItemArray[0];

                //var paramNames = new string[] { "@id" };
                //var values = new object[] { id };

                //dal.CallProcedureWithParameters(paramNames, values, "usp_DeleteCaretaker");

                //caretakerTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadCaretaker").DefaultView;
            //}


        }
    }
}
