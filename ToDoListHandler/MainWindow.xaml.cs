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
        private string callingProcess;

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
                } catch
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
            string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(workingDirectory, tmpFileName, SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                res = files.First();
            }
            else
            {
                //No jsonFile yet. Create a new one:
                Directory.CreateDirectory(workingDirectory + "Data");
                res = workingDirectory + $"Data\\{tmpFileName}";
            }
            return res;
        }

        private void dataGrid_todo_RowEditEnding(object sender, System.Windows.Controls.DataGridRowEditEndingEventArgs e)
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
            } catch
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


        private void dataGrid_todo_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);

                Control control = GetFirstChildByType<Control>(e.OriginalSource as DataGridCell);
                if (control != null)
                {
                    control.Focus();
                }
            }
        }


        private T GetFirstChildByType<T>(DependencyObject prop) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(prop); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild((prop), i) as DependencyObject;
                if (child == null)
                    continue;

                T castedProp = child as T;
                if (castedProp != null)
                    return castedProp;

                castedProp = GetFirstChildByType<T>(child);

                if (castedProp != null)
                    return castedProp;
            }
            return null;
        }

        
    }
}
