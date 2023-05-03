using Data_Access_Entity;
using Data_Access_Entity.Entities;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Waiter_Main;
using static System.Net.Mime.MediaTypeNames;

namespace Waiter_App
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        ViewModel ViewModel = new ViewModel();
        RestaurantContext restaurantContext = new RestaurantContext();
        public Orders()
        {
            InitializeComponent();
            DbToViewModel();
            GetCategoriesToComboBox();
            GetTablesToComboBox();
            DataContext = ViewModel;

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
            System.Windows.Application.Current.Shutdown();
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        #endregion

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow menu = new MainWindow();
            this.Close();
            menu.ShowDialog();
        }
        private void ComboBoxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBoxProductsFromMenu.Items.Clear();
                Category selectedCategory = ViewModel.Category.FirstOrDefault(a => a.Name == (string)ComboBoxCategories.SelectedValue);
                foreach (var item in ViewModel.Product)
                {
                    if (item.CategoryId == selectedCategory.ID)
                    {
                        ListBoxProductsFromMenu.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBoxTables.SelectedValue != null && ListBoxProductsFromMenu.SelectedItem != null)
                {
                    Order thisorder = ViewModel.Orders.FirstOrDefault(o => o.Active == false && o.TableId == (int)ComboBoxTables.SelectedValue);
                    Product selectedvalue = (Product)ListBoxProductsFromMenu.SelectedItem;

                    if (thisorder != null)
                    {
                        ViewModel.AddInProductOrder(new ProductOrder
                        {
                            OrderId = thisorder.ID,
                            ProductId = selectedvalue.ID
                        });
                    }
                    else
                    {
                        restaurantContext.Orders.Add(new Order
                        {
                            Active = false,
                            OrderDate = DateTime.Now,
                            TableId = (int)ComboBoxTables.SelectedValue,
                            WaiterId = User.ID
                        });
                        restaurantContext.SaveChanges();
                        var newOrder = restaurantContext.Orders.FirstOrDefault(o => o.Active == false && o.TableId == (int)ComboBoxTables.SelectedValue);
                        ViewModel.AddInOrders(new Order
                        {
                            ID = newOrder.ID,
                            Active = false,
                            OrderDate = DateTime.Now,
                            TableId = (int)ComboBoxTables.SelectedValue,
                            WaiterId = User.ID
                        });
                        ViewModel.AddInProductOrder(new ProductOrder
                        {
                            OrderId = newOrder.ID,
                            ProductId = selectedvalue.ID
                        });
                    }
                    GetOrderItems();
                }
                else
                    MessageBox.Show($@"PLease, make your choise first");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ComboBoxTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                GetOrderItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetCategoriesToComboBox()
        {
            try
            {
                foreach (var item in ViewModel.Category)
                {
                    ComboBoxCategories.Items.Add(item.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetTablesToComboBox()
        {
            try
            {
                foreach (var item in ViewModel.Table)
                {
                    ComboBoxTables.Items.Add(item.ID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetOrderItems()
        {
            ListBoxProductsFromOrderByTableNumber.Items.Clear();
            Order thisorder = ViewModel.Orders.FirstOrDefault(o => o.Active == false && o.TableId == (int)ComboBoxTables.SelectedValue);
            if (thisorder != null)
            {
                var b = ViewModel.GetProductId(thisorder.ID);
                List<Product> Show = new List<Product>();
                foreach (var item in b)
                {
                    Show.Add(ViewModel.Product.FirstOrDefault(x => x.ID == item));
                }
                foreach (var item in Show)
                    ListBoxProductsFromOrderByTableNumber.Items.Add(item);
            }
        }

        private void GetProducts()
        {
            try
            {
                var products = restaurantContext.Products;
                foreach (var item in products)
                {
                    ViewModel.AddInProduct(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetCategories()
        {
            try
            {
                var categories = restaurantContext.Categories;
                foreach (var item in categories)
                {
                    ViewModel.AddInCategory(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetTables()
        {
            try
            {
                var tables = restaurantContext.Tables;
                foreach (var item in tables)
                {
                    if(item.WaiterId == User.ID)
                        ViewModel.AddInTable(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DbToViewModel()
        {
            GetCategories();
            GetProducts();
            GetTables();
        }
    }
}
