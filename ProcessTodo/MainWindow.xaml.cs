using Microsoft.Win32;
using ProcessTodo.Classes;
using ProcessTodo.Classes.Objects;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProcessTodo
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    ///

    //TODO: Scale all UI elements up
    //TODO: Better embedding in Process Window Host
    //TODO: Auto Terminate when host closes

    public partial class MainWindow : Window
    {
        private readonly TaskSched_Communicator t_handler;

        public MainWindow()
        {
            InitializeComponent();

            Check_auditpol();

            t_handler = new TaskSched_Communicator();
            UpdateList();
        }

        private void Check_auditpol()
        {
            AuditPol_HND auditPol_HND = new AuditPol_HND();

            if (!auditPol_HND.IsTrackingPolicySet())
            {
                //Need to set auditpol!
                new window_auditpol().ShowDialog();
            }
            else
            {
                //auditpol allready set
            }
        }

        private void UpdateList()
        {
            listBox_Tasks.Items.Clear();

            List<TodoList> todoLists = TodoListManager.GetListFromJson();
            if (todoLists == null)
                return;

            foreach (TodoList tList in todoLists)
            {
                CheckBox cb = new CheckBox();
                cb.SetResourceReference(Control.StyleProperty, "CheckBoxStyle_Dark");
                //format the name
                //string tmpName = "lb_task_item_" + .Name.Replace("[PTD]", "").Replace(" ", "").Replace("-", "").Split('.').First();
                cb.Tag = tList;
                cb.Content = tList.DisplayName;
                cb.IsChecked = false;
                listBox_Tasks.Items.Add(cb);
            }
        }

        private void Button_reg_new_process_Click(object sender, RoutedEventArgs e)
        {
            //TODO: implement, einschalten der prozessüberwachung als GUI.
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".exe";
            dlg.Filter = "Executables (*.exe)|*.exe|All Files (*.*)|*.*";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string processToRegister = dlg.FileName;
                string taskName = processToRegister.Replace("\\", "_").Replace(":", "_");
                string xamlPath = Constants.todoListDataFolder + taskName + ".xaml";
                string processId = taskName + new Random().Next();
                TodoList todoList = new TodoList() { DisplayName = dlg.FileName, TaskName = taskName, XamlFilePath = xamlPath, Id = processId };

                bool exec = t_handler.CreateTask(processToRegister, taskName, processId);
                if (exec)
                {
                    TodoListManager.AddTodoList(todoList); //Adds the new TodoList to the json file
                }
                else
                {
                    MessageBox.Show("Failed to register task");
                }
            }
            UpdateList();
        }

        private void Button_showTodoList_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in listBox_Tasks.Items)
            {
                CheckBox tmp = (CheckBox)item;
                if (tmp.IsChecked == true)
                {
                    if (!t_handler.RunTask(((TodoList)tmp.Tag).TaskName))
                    {
                        MessageBox.Show("Task could not be executed");
                    }
                }
            }
        }

        private void Button_delete_selected_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0;

            foreach (CheckBox cb in listBox_Tasks.Items)
            {
                if (cb.IsChecked == true)
                {
                    TodoList selectedTodoList = (TodoList)cb.Tag;
                    List<TodoList> listTemp = TodoListManager.GetListFromJson();

                    t_handler.DeleteTask(selectedTodoList.TaskName);

                    if (FileSystem.DeleteTodoListFile(selectedTodoList.XamlFilePath))
                        counter++;

                    //update the json file (with the deleted todolists)
                    listTemp.RemoveAll((x) => x.TaskName.Equals(selectedTodoList.TaskName));
                    TodoListManager.UpdateJsonFile(listTemp);
                }
            }
            MessageBox.Show($"Deleted {counter} Todo-Lists");
            UpdateList();
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

        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragMove();
        }

        private void Button_maximize_Click(object sender, RoutedEventArgs e)
        {
            Switch_window_size();
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void Button_minimize_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                Switch_window_size();
        }
    }
}