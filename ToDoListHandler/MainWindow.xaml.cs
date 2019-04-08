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

        private string XamlPath;

        private DataTable dataTable = new DataTable();

        public MainWindow()
        {
            try
            {
                callingProcess = Environment.GetCommandLineArgs().Last().Replace("caller=", "");
            }
            catch { }

            InitializeComponent();

            initializeTextBox();

            this.XamlPath = findXamlfile();
            this.data_Handler = new Data_handler(); 
            //this.data_UserInterface = new Data_userInterface(dataTable, this.XamlPath);
            this.data_UserInterface = new Data_userInterface(rich_textbox_1, this.XamlPath);

            label_todoList_title.Content = callingProcess;

            updateGrid();
        }

        private void updateGrid()
        {
            data_UserInterface.updateTextBox();
        }

        private void saveGrid()
        {
            TodoListClass todoListClass = new TodoListClass();
            todoListClass.document = rich_textbox_1.Document;
            data_Handler.saveXamlObject(todoListClass, this.XamlPath);
        }

        private void initializeTextBox()
        {

        }

        private string findXamlfile()
        {
            string callingProcessClean = callingProcess.Replace("\\", "").Replace(":", "");
            string tmpFileName = $"{callingProcessClean}.xaml";
            string res = "";

            string XamlFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\processtodo_todolists\\";

            if (!Directory.Exists(XamlFolderPath))
                Directory.CreateDirectory(XamlFolderPath);


            string[] files = Directory.GetFiles(XamlFolderPath, tmpFileName, SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                res = files.First();
            }
            else
            {
                try
                {
                    res = XamlFolderPath + tmpFileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return res;
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
            saveGrid();
            Environment.Exit(1);
        }

        private void button_minimize_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void todoList_mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveGrid();
        }
    }
}
