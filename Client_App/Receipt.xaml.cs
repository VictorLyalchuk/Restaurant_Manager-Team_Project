using Data_Access_Entity;
using Data_Access_Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client_App
{
    public partial class Receipt : Window
    {
        ViewModel model = new ViewModel();
        Waiter Waiter;
        public Receipt(List<Product> products, Waiter waiter, double price)
        {
            InitializeComponent();
            Waiter = waiter;
            foreach (var item in products)
                model.AddInProduct(item);   
            FullName.Text = $"{waiter.FirstName} {waiter.SurName}";
            TotalPrice.Text = $"{Math.Round(price,2)}$";
            DataContext = model;
            
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (BasicRatingBar.Value == 0) Close();
            int rating = Waiter.Raiting + Convert.ToInt32(BasicRatingBar.Value);
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                if (Waiter.Raiting > 0)
                    restaurantContext.Waiters.FirstOrDefault(x => x.ID == Waiter.ID).Raiting = rating / 2;
                else
                    restaurantContext.Waiters.FirstOrDefault(x => x.ID == Waiter.ID).Raiting = rating;
                restaurantContext.SaveChanges();
            }
            Close();
        }
    }
}
