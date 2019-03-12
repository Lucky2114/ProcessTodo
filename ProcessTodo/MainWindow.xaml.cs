using ProcessTodo.Classes;
using System;
using System.Collections.Generic;
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
using Microsoft.Win32.TaskScheduler;
using System.IO;

namespace ProcessTodo
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 

    public class CheckedItem
    {
        public string Text { get; set; }
        public bool IsChecked { get; set; }
    }

    public partial class MainWindow : Window
    {
        private TaskSched_Handler t_handler;
        public MainWindow()
        {
            InitializeComponent();
            t_handler = new TaskSched_Handler();

            updateList();
        }


        private void updateList()
        {
            List<Microsoft.Win32.TaskScheduler.Task> tasks = t_handler.getTasks();

            List<CheckedItem> items = new List<CheckedItem>();
            foreach (Microsoft.Win32.TaskScheduler.Task task in tasks)
            {
                //format the name
                string tmpName = "lb_task_item_" + task.Name.Replace("[PTD]", "").Replace(" ", "").Replace("-", "").Split('.').First();
                items.Add(new CheckedItem() { Text = task.Name, IsChecked = false });
            }

            listBox_Tasks.ItemsSource = items;
        }

        private void button_reg_new_process_Click(object sender, RoutedEventArgs e)
        {
            string processToRegister = null;
            //TODO: implement, einschalten der prozessüberwachung als GUI.
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".exe";
            dlg.Filter = "Executables (*.exe)|*.exe|All Files (*.*)|*.*";
            
            bool? result = dlg.ShowDialog();
            
            if (result == true)
            {
                processToRegister = dlg.FileName;
                bool exec = t_handler.createTask(processToRegister, "[PTD] - " + processToRegister.Replace("\\", "_").Replace(":", "_"));
                if (exec)
                {
                    MessageBox.Show("Task Sucessfully Registered");
                } else
                {
                    MessageBox.Show("Task Registering Failed.");
                }
            }
            updateList();
        }

        private void button_delete_selected_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedTaskNames = new List<string>();
            
            foreach (CheckedItem ci in listBox_Tasks.Items)
            {
                if (ci.IsChecked)
                    selectedTaskNames.Add(ci.Text);
            }
            int counter = 0;
            foreach (string t in selectedTaskNames)
            {
                t_handler.deleteTask(t);
                
                if (new FileSystem().deleteTodoListFile(t))
                    counter += 1;
                    
            }
            MessageBox.Show($"Deleted {counter} Todo-Lists");
            updateList();
        }

        private void button_reg_new_process_Copy_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in listBox_Tasks.Items)
            {
                CheckedItem tmp = (CheckedItem)item;
                if (tmp.IsChecked)
                {
                    if (!t_handler.runTask(tmp.Text))
                    {
                        MessageBox.Show("Task could not be executed");
                    }
                }
            }
        }

        private void switch_window_size()
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

        private void button_maximize_Click(object sender, RoutedEventArgs e)
        {
            switch_window_size();
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void button_minimize_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                switch_window_size();

        }
    }
}
