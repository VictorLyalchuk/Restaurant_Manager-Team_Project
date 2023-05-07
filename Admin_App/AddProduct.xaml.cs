using Data_Access_Entity;
using Data_Access_Entity.Entities;
using Microsoft.EntityFrameworkCore;
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
using Waiter_App;

namespace Admin_App
{
    public partial class AddProduct : Window
    {
        ViewModel viewmodel = new ViewModel();
        bool Edit;
        int IDP;
        public AddProduct()
        {
            InitializeComponent();
            SetModel();
            DataContext = viewmodel;
            Edit = false;
        }
        public AddProduct(Product product)
        {
            InitializeComponent();
            SetModel();
            SetInfo(product);
            DataContext = viewmodel;
            Edit = true;
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            this.Close();
            menu.Show();
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text != "" && TextBoxPrice.Text != "" && ComboBoxCategories.SelectedItem != null)
            {
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    Category selcategory = (Category)ComboBoxCategories.SelectedItem;
                    if (!Edit)
                    {
                        restaurantContext.Products.Add(new Product
                        {
                            Name = TextBoxName.Text,
                            Price = double.Parse(TextBoxPrice.Text),
                            CategoryId = selcategory.ID,
                        });
                        restaurantContext.SaveChanges();
                        MessageBox.Show("Product successfully added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        restaurantContext.Products.FirstOrDefault(w => w.ID == IDP)!.Name = TextBoxName.Text;
                        restaurantContext.Products.FirstOrDefault(w => w.ID == IDP)!.Price = double.Parse(TextBoxPrice.Text);
                        restaurantContext.Products.FirstOrDefault(w => w.ID == IDP)!.CategoryId = selcategory.ID;
                        restaurantContext.SaveChanges();
                        MessageBox.Show("Product updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
                MessageBox.Show("Please fill in all values", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void SetModel()
        {
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                var caegories = restaurantContext.Categories.Include(c => c.Products);
                foreach (var item in caegories)
                {
                    viewmodel.AddInCategory(item);
                }
            }
        }
        private void SetInfo(Product product)
        {
            Category selCat = null;
            foreach (var item in viewmodel.Category)
            {
                if (item.ID == product.CategoryId)
                    selCat = item;
            }
            IDP = product.ID;
            TextBoxName.Text = product.Name;
            TextBoxPrice.Text = product.Price.ToString();
            ComboBoxCategories.SelectedValue = selCat;
        }
    }
}
