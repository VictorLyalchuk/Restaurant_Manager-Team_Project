using Data_Access_Entity;
using Data_Access_Entity.Entities;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Waiter_App;

namespace Admin_App
{
    public partial class Menu : Window
    {
        ViewModel viewmodel = new ViewModel();
        public Menu()
        {
            InitializeComponent();
            SetModel();
            DataContext = viewmodel;
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

        private void finalDatePicker_selectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (finalDatePicker.SelectedDate < startDatePicker.SelectedDate) { MessageBox.Show("Choose only future dates!"); finalDatePicker.SelectedDate = DateTime.Now; }
        }

        private void startDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (startDatePicker.SelectedDate > DateTime.Now) { MessageBox.Show("Choose only former date!"); startDatePicker.SelectedDate = DateTime.Now; }
        }
        private void AddWaiterBtn_Click(object sender, RoutedEventArgs e)
        {
            AddWaiter add = new AddWaiter();
            this.Close();
            add.Show();
        }
        private void Edit_Waiter_Button(object sender, RoutedEventArgs e)
        {
            Waiter result = (Waiter)MyDataGrid.SelectedItem;
            AddWaiter add = new AddWaiter(result);
            this.Close();
            add.Show();
        }
        private void Delete_Selected_Waiter_Button(object sender, RoutedEventArgs e)
        {
            Waiter result = (Waiter)MyDataGrid.SelectedItem;
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                restaurantContext.Waiters.Remove(result);
                restaurantContext.SaveChanges();
            }
            viewmodel.RemoveInWaiters(result);

            DeleteWaiter.IsEnabled = false;
        }
        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteWaiter.IsEnabled = true;
            EditWaiterButton.IsEnabled = true;
        }
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProduct add = new AddProduct();
            this.Close();
            add.Show();
        }
        private void Edit_Product_Button(object sender, RoutedEventArgs e)
        {
            Product result = (Product)MyDataGrid2.SelectedItem;
            AddProduct add = new AddProduct(result);
            this.Close();
            add.Show();
        }
        private void Delete_Product_Button(object sender, RoutedEventArgs e)
        {
            Product product = (Product)MyDataGrid2.SelectedItem;
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                restaurantContext.Products.Remove(product);
                restaurantContext.SaveChanges();
            }
            viewmodel.RemoveInProduct(product);

            DeleteWaiter.IsEnabled = false;
        }
        private void MyDataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditProductButton.IsEnabled = true;
            DeleteProductButton.IsEnabled = true;
        }
        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategory add = new AddCategory();
            this.Close();
            add.Show();
        }
        private void Edit_Category_Button(object sender, RoutedEventArgs e)
        {
            Category result = (Category)MyDataGrid3.SelectedItem;
            AddCategory add = new AddCategory(result);
            this.Close();
            add.Show();
        }
        private void Delete_Category_Button(object sender, RoutedEventArgs e)
        {
            Category category = (Category)MyDataGrid3.SelectedItem;
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                restaurantContext.Categories.Remove(category);
                restaurantContext.SaveChanges();
            }
            viewmodel.RemoveInCategory(category);

            DeleteCategoryButton.IsEnabled = false;
        }
        private void MyDataGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditCategoryButton.IsEnabled = true;
            DeleteCategoryButton.IsEnabled = true;
        }
        private void View_Waiters_Button(object sender, RoutedEventArgs e)
        {
            List<Waiter> result;
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                result = restaurantContext.Waiters.Include(w => w.Tables).ToList();
            }
            MyDataGrid.ItemsSource = result;
        }
        private void View_Products_Button(object sender, RoutedEventArgs e)
        {
            List<Product> result;
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {

                result = restaurantContext.Products.Include(p => p.Category).ToList();
            }
            MyDataGrid2.ItemsSource = result;
        }
        private void View_Categories_Button(object sender, RoutedEventArgs e)
        {
            List<Category> result;
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {

                result = restaurantContext.Categories.Include(c => c.Products).ToList();
            }
            MyDataGrid3.ItemsSource = result;
        }
        private void SetModel()
        {
            try
            {
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    var waiters = restaurantContext.Waiters.Include(a => a.Tables);
                    var products = restaurantContext.Products.Include(p => p.Category);
                    var caegories = restaurantContext.Categories.Include(c => c.Products);
                    var orders = restaurantContext.Orders.Include(c => c.Waiter);
                    foreach (var item in waiters)
                    {
                        viewmodel.AddInWaiter(item);
                    }
                    foreach (var item in products)
                    {
                        viewmodel.AddInProduct(item);
                    }
                    foreach (var item in caegories)
                    {
                        viewmodel.AddInCategory(item);
                    }
                    foreach (var item in orders)
                    {
                        viewmodel.AddInOrders(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void View_Finances_Button(object sender, RoutedEventArgs e)
        {
            if (startDatePicker.SelectedDate != null && finalDatePicker.SelectedDate != null)
            {
                double sum = 0;
                ObservableCollection<Order> result = new ObservableCollection<Order>();
                DateTime endtime = (DateTime)finalDatePicker.SelectedDate;
                DateTime endtime2 = new DateTime(endtime.Year, endtime.Month, endtime.Day, 23, 59, 59);
                foreach (var item in viewmodel.Orders)
                {
                    if (item.OrderDate >= startDatePicker.SelectedDate.Value && item.OrderDate <= endtime2)
                        result.Add(item);
                }
                MyDataGrid4.ItemsSource = result.ToList();
                foreach (var item in result)
                {
                    sum += item.TotalSum;
                }
                TotalSumOrders.Content = sum + " $ | Total";
            }
        }
    }
}
