using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToDoListHandler.Classes;
using ToDoListHandler.Classes.JSON;

namespace ToDoListHandler
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string callingProcess = "";

        private Data_userInterface data_UserInterface;
        private Data_handler data_Handler;

        private string jsonPath;

        private DataTable dataTable = new DataTable();

        public MainWindow()
        {
            try
            {
                callingProcess = Environment.GetCommandLineArgs().Last().Replace("caller=", "");
            }
            catch { }

            InitializeComponent();

            initializeGrid();

            this.jsonPath = findJsonfile();
            this.data_Handler = new Data_handler();
            this.data_UserInterface = new Data_userInterface(dataTable, this.jsonPath);

            label_todoList_title.Content = callingProcess;

            updateGrid();
        }

        private void updateGrid()
        {
            this.dataTable = data_UserInterface.updateDataTable();
        }

        private void saveGrid()
        {
            //Get the List from the grid
            List<TodoListClass> rows = new List<TodoListClass>();
            DataRowCollection r = dataTable.Rows;

            for (int i = 0; i < r.Count; i++)
            {
                TodoListClass todoListClass = new TodoListClass();
                try
                {
                    todoListClass.status = (bool)r[i][0];
                }
                catch
                {
                    todoListClass.status = false;
                }
                todoListClass.todoItem = r[i][1].ToString();
                rows.Add(todoListClass);
            }

            data_Handler.saveJsonObject(rows, this.jsonPath);
        }

        private void initializeGrid()
        {
            dataTable.Columns.Add("Status", typeof(bool));
            dataTable.Columns.Add("Item", typeof(string));

            dataGrid_todo.DataContext = dataTable.DefaultView;
        }

        private string findJsonfile()
        {
            string callingProcessClean = callingProcess.Replace("\\", "").Replace(":", "");
            string tmpFileName = $"{callingProcessClean}.json";
            string res = "";

            string jsonFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\processtodo_todolists\\";

            if (!Directory.Exists(jsonFolderPath))
                Directory.CreateDirectory(jsonFolderPath);
           

            string[] files = Directory.GetFiles(jsonFolderPath, tmpFileName, SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                res = files.First();
            }
            else
            {
                try
                {
                    res = jsonFolderPath + tmpFileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return res;
        }

        private void dataGrid_todo_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            saveGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            saveGrid();
        }

        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                App.Current.MainWindow.DragMove();
            }
            catch
            {
                //Rechte Maustaste
            }
        }

        private void button_maximize_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow.WindowState == WindowState.Maximized)
                App.Current.MainWindow.WindowState = WindowState.Normal;
            else if (App.Current.MainWindow.WindowState == WindowState.Normal)
                App.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void button_minimize_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void todoList_mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid_todo.Columns[0].Width = DataGridLength.Auto;

        }


        private void button_delete_checked_Click(object sender, RoutedEventArgs e)
        {
            List<DataRow> rowsToDelete = new List<DataRow>();
            foreach (DataRow item in dataTable.Rows)
            {
                try
                {
                    if ((bool)item[0])
                    {
                        rowsToDelete.Add(item);
                    }
                }
                catch
                {
                    //No conversion possible
                }
            }

            foreach (var item in rowsToDelete)
            {
                dataTable.Rows.Remove(item);
            }
        }
    }
}
