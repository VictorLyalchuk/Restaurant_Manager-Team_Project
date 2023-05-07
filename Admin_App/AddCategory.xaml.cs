using Data_Access_Entity;
using Data_Access_Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
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
    public partial class AddCategory : Window
    {
        bool Edit;
        int IDC;
        public AddCategory()
        {
            InitializeComponent();
            Edit = false;
        }
        public AddCategory(Category category)
        {
            InitializeComponent();
            SetInfo(category);
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

        private void Add_Button(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text != "")
            {
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    if (!Edit)
                    {
                        restaurantContext.Categories.Add(new Category
                        {
                            Name = TextBoxName.Text,
                        });
                        restaurantContext.SaveChanges();
                        MessageBox.Show("Category successfully added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        restaurantContext.Categories.FirstOrDefault(w => w.ID == IDC)!.Name = TextBoxName.Text;
                        restaurantContext.SaveChanges();
                        MessageBox.Show("Category updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
                MessageBox.Show("Please fill in all values", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void SetInfo(Category category)
        {
            IDC = category.ID;
            TextBoxName.Text = category.Name;
        }
    }
}
