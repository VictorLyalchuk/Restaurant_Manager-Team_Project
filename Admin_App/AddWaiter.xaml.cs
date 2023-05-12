using Data_Access_Entity;
using Data_Access_Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

namespace Admin_App
{
    public partial class AddWaiter : Window
    {
        bool Edit;
        int IDW;
        public AddWaiter()
        {
            InitializeComponent();
            SetTables();
            Edit = false;
        }
        public AddWaiter(Waiter waiter)
        {
            InitializeComponent();
            SetInfo(waiter);
            SetTables();
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

        private void Add_Waiter_Button(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text != "" && TextBoxSurName.Text != "" && TextBoxSalary.Text != "" && PickerBirthDate.SelectedDate != null && PickerAcceptDate.SelectedDate != null)
            {
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    if (!Edit)
                    {
                        restaurantContext.Waiters.Add(new Waiter
                        {
                            FirstName = TextBoxName.Text,
                            SurName = TextBoxSurName.Text,
                            Salary = double.Parse(TextBoxSalary.Text),
                            BirthDate = PickerBirthDate.SelectedDate.Value,
                            StartWorkingDate = PickerAcceptDate.SelectedDate.Value
                        });
                        restaurantContext.SaveChanges();
                        IDW = restaurantContext.Waiters.OrderBy(a => a.ID).LastOrDefault()!.ID;
                        MessageBox.Show("Waiter successfully added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        restaurantContext.Waiters.FirstOrDefault(w => w.ID == IDW)!.FirstName = TextBoxName.Text;
                        restaurantContext.Waiters.FirstOrDefault(w => w.ID == IDW)!.SurName = TextBoxSurName.Text;
                        restaurantContext.Waiters.FirstOrDefault(w => w.ID == IDW)!.Salary = double.Parse(TextBoxSalary.Text);
                        restaurantContext.Waiters.FirstOrDefault(w => w.ID == IDW)!.BirthDate = PickerBirthDate.SelectedDate.Value;
                        restaurantContext.Waiters.FirstOrDefault(w => w.ID == IDW)!.StartWorkingDate = PickerAcceptDate.SelectedDate.Value;

                        restaurantContext.SaveChanges();
                        MessageBox.Show("Waiter updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                UpdateTableInWaiter();
            }
            else
                MessageBox.Show("Please fill in all values", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        private void SetInfo(Waiter waiter)
        {
            IDW = waiter.ID;
            TextBoxName.Text = waiter.FirstName;
            TextBoxSurName.Text = waiter.SurName;
            TextBoxSalary.Text = waiter.Salary.ToString();
            PickerBirthDate.SelectedDate = waiter.BirthDate;
            PickerAcceptDate.SelectedDate = waiter.StartWorkingDate;
            foreach (var item in waiter.Tables)
            {
                TablesLB.Items.Add(item.ID);
                TablesLB.SelectedItems.Add(item.ID);
            }
        }
        private void SetTables()
        {
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                foreach (var item in restaurantContext.Tables)
                {
                    if (item.WaiterId == null)
                        TablesLB.Items.Add(item.ID);
                }
            }
        }
        private void UpdateTableInWaiter()
        {
            try
            {
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    var collTable = restaurantContext.Tables.Where(t => t.WaiterId == IDW);
                    foreach (var item in collTable)
                    {
                        item.WaiterId = null;
                    }
                    var selectedTables = TablesLB.SelectedItems;
                    foreach (var item in selectedTables)
                    {
                        restaurantContext.Tables.FirstOrDefault(t => t.ID == (int)item)!.WaiterId = IDW;
                    }
                    restaurantContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
