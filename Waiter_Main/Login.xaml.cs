﻿using Data_Access_Entity;
using Data_Access_Entity.Entities;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Waiter_Main;

namespace Waiter_App
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        ViewModel ViewModel = new ViewModel();
        public Login()
        {
            InitializeComponent();
            GetWaiter();
            GetAccount();
        }
        #region adaptive borderless-window react
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);

        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        #endregion
        #region navigation bar buttons

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        #endregion

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Waiter waiter = ViewModel.Waiter.FirstOrDefault(x => x.FirstName == username.SelectedItem.ToString() &&
                                                x.SurName == usersurname.SelectedItem.ToString())!;
            //Orders orders = new Orders(waiter.ID);
            User.ID = waiter.ID;
            Orders orders = new Orders();
            this.Close();
            orders.ShowDialog();
        }

        private void username_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (username.SelectedItem != null) usersurname.IsEnabled = true;
            else { usersurname.IsEnabled = false; return; }
            var waiter = ViewModel.Waiter.Where(x => x.FirstName == username.SelectedItem.ToString())!;
            usersurname.Items.Clear();
            foreach (var item in waiter)
            {
                usersurname.Items.Add(item.SurName);
            }
        }
        private void usersurname_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersurname.SelectedItem != null && username.SelectedItem != null) btnLogin.IsEnabled= true;
            else { usersurname.IsEnabled = false;}
        }
        private void GetAccount()
        {
            var waiter = ViewModel.Waiter;
            foreach (var waiterItem in waiter)
            {
                username.Items.Add(waiterItem.FirstName);
                usersurname.Items.Add(waiterItem.SurName);
            }
        }
        private void GetWaiter()
        {
            try
            {
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    var waiters = restaurantContext.Waiters;
                    foreach (var item in waiters)
                    {
                        ViewModel.AddInWaiter(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
