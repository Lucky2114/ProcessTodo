﻿using ProcessTodo.Classes;
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
using System.Windows.Shapes;

namespace ProcessTodo
{
    /// <summary>
    /// Interaktionslogik für window_auditpol.xaml
    /// </summary>
    public partial class window_auditpol : Window
    {
        public window_auditpol()
        {
            InitializeComponent();


        }

        private void button_enable_auditpol_Click(object sender, RoutedEventArgs e)
        {
            if (new AuditPol_HND().setTrackingPolicy(true))
            {
                MessageBox.Show("Audit Tracking Policy Sucessfully Enabled");
                this.Close();
            } else
            {
                MessageBox.Show("Audit Tracking Policy Not Enabled");
                Environment.Exit(1);
            }
        }

    



        private void switch_window_size()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
        }


        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
       

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                switch_window_size();

        }


        private void button_minimize_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void button_close_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
