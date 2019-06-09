using System;
using System.IO;
using System.Linq;
using System.Windows;
using ToDoListHandler.Classes;
using ToDoListHandler.Classes.JSON;
using ToDoListHandler.Classes.Objects;

namespace ToDoListHandler
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string callingProcessId = "";

        private readonly Data_userInterface data_UserInterface;
        private readonly Data_handler data_Handler;

        private readonly string XamlPath;

        private readonly TodoList TodoList;

        public MainWindow()
        {
            try
            {
                callingProcessId = Environment.GetCommandLineArgs().Last().Replace("caller=", "");
            }
            catch { }

            InitializeComponent();

            this.TodoList = GetTodoListFromJson();

            this.XamlPath = FindOrCreateXamlfile();
            this.data_Handler = new Data_handler();
            this.data_UserInterface = new Data_userInterface(rich_textbox_1, this.XamlPath);

            label_todoList_title.Content = TodoList?.DisplayName;

            UpdateGrid();
        }

        private void UpdateGrid()
        {
            data_UserInterface.UpdateTextBox();
        }

        private void SaveGrid()
        {
            TodoListClass todoListClass = new TodoListClass
            {
                Document = rich_textbox_1.Document
            };
            data_Handler.SaveXamlObject(todoListClass, this.XamlPath);
        }

        private TodoList GetTodoListFromJson()
        {
            return TodoListManager.FindTodoListFromJson(callingProcessId);
        }

        private string FindOrCreateXamlfile()
        {
            if (TodoList != null)
            {
                if (!File.Exists(TodoList.XamlFilePath))
                    File.Create(TodoList.XamlFilePath).Close();
                return TodoList.XamlFilePath;
            }
            else return "";
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveGrid();
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

        private void Switch_window_size()
        {
            if (App.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                App.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else if (App.Current.MainWindow.WindowState == WindowState.Normal)
            {
                App.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void Button_maximize_Click(object sender, RoutedEventArgs e)
        {
            Switch_window_size();
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            SaveGrid();
            Environment.Exit(0);
        }

        private void Button_minimize_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void TodoList_mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveGrid();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                Switch_window_size();
        }
    }
}