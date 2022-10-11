﻿using Microsoft.Data.SqlClient;
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
using System.Windows.Threading;

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

        private void OnAddButton(object sender, RoutedEventArgs e)
        {
            var name = nameTextBox.Text.Trim();
            var phoneNumber = phoneNbrTextBox.Text.Trim();
            var paramNames = new string[] { "@name", "@phoneNbr" };
            var values = new object[] { name, phoneNumber };

            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phoneNumber))
                {
                    MessageBox.Show("Please add information");
                }
                else
                {
                    dal.CallProcedureWithParameters(paramNames, values, "usp_CreateCaretaker");

                    caretakerTable.ItemsSource = dal.ReadByStoredProcedure("usp_ReadCaretaker").DefaultView;
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
            phoneNbrTextBox.Clear();
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
            var row = e.Row as DataGridRow;
            if (row.Item is DataRowView rowView)
            {
                object?[] itemArray = rowView.Row.ItemArray;
                var paramNames = new string[] { "@id", "@name", "@phoneNumber" };

                dal.CallProcedureWithParameters(paramNames, itemArray, "usp_UpdateCaretaker");

            }

        }
    }
}
