using Data_Access_Entity;
using Data_Access_Entity.Entities;
using LibraryForServer;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using Waiter_App.ViewModel_Models;
using Application = System.Windows.Application;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Waiter_App
{
    public partial class Orders : Window
    {
        ViewModel ViewModel = new ViewModel();
        static int WaiterID;
        IPEndPoint serverEndPoint;
        UdpClient client;
        public Orders()
        {
            InitializeComponent();
            DbToViewModel();
            SetWaiterProfile();
            DataContext = ViewModel;
            WaiterID = User.ID;
            #region Connect to server
            client = new UdpClient();
            string serverAddress = ConfigurationManager.AppSettings["ServerAddress"]!;
            short serverPort = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
            SendMessage(new LogicClassToRecipient { Function = "$WAITERJOIN", Id = WaiterID });
            ListenAsync();
            #endregion
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
            this.Close();
            //System.Windows.Application.Current.Shutdown();
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        #endregion
        #region Function For Server_App
        private async void SendMessage(object message)
        {
            byte[] data;
            BinaryFormatter formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, message);
                data = ms.ToArray();
            }
            await client.SendAsync(data, data.Length, serverEndPoint);
        }

        async void ListenAsync()
        {
            await Task.Run(() => {

                while (true)
                {
                    byte[] data = client.Receive(ref serverEndPoint);
                    LogicClass logic = (LogicClass)ConvertFromBytes(data);

                    if (logic.Function == "$ADDORDER")
                    {
                        LogicClassToOrders classToOrders = (LogicClassToOrders)logic;
                        if (ViewModel.Orders.FirstOrDefault(x => x.ID == classToOrders.order.ID) == null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ViewModel.AddInNew(new StringClass() { Message = classToOrders.Msg });
                                ViewModel.AddInOrders(classToOrders.order);
                                ViewModel.Table.FirstOrDefault(x => x.ID == classToOrders.order.TableId).Active = false;
                            });
                        }
                        else
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ViewModel.AddInUpdate(new StringClass() { Message = $"• Client at table {classToOrders.order.TableId} update order" });
                            });

                        }
                        foreach (var item in classToOrders.products)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {                                
                                ViewModel.AddInProductOrder(item);
                            });
                        }
                    }
                    else if (logic.Function == "$SENDMESSAGE_TO_WAITER")
                    {
                        LogicClassToCheck logicClassToCheck = (LogicClassToCheck)logic;
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            ViewModel.AddInReceipts(new IDClass { TableId = logicClassToCheck.TableID, OrderID = logicClassToCheck.OrderId });
                        });
                    }
                    else if (logic.Function == "$SENDMESSAGE")
                    {
                        LogicClassToMessage logicClassToMessage = (LogicClassToMessage)logic;
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            ViewModel.AddInNew(new StringClass { Message = logicClassToMessage.Message });
                        });
                    }
                }

            });
        }
        public object ConvertFromBytes(byte[] data)
        {
            using (var memStream = new MemoryStream())
            {
                try
                {
                    var binForm = new BinaryFormatter();
                    memStream.Write(data, 0, data.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    object obj = binForm.Deserialize(memStream);
                    return obj;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        #endregion
        private void SetWaiterProfile()
        {
            try
            {
                var w = ViewModel.Waiter.Where(x => x.ID == User.ID).FirstOrDefault();
                CurrentWaiterLabel.Content = w.FirstName + " " + w.SurName;
                var dir = Directory.GetCurrentDirectory() + @"\waiters";
                var pics = Directory.GetFiles(dir);
                string path = @"/img/waiters/";
                foreach (var item in pics)
                {
                    if (System.IO.Path.GetFileNameWithoutExtension(item) == User.ID.ToString())
                    {
                        WaiterPicture.Source = new BitmapImage(new Uri(path + System.IO.Path.GetFileName(item), UriKind.Relative));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }
        private void ActiveOrderLBItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Orders orders = new Orders();
            orders.Show();
        }
        private void ListBoxCheck_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Order order = ViewModel.Orders.FirstOrDefault(x => x.ID == ViewModel.SelectedRecepient.OrderID)!;
            var collection = ViewModel.GetProductId(order.ID);
            ViewModel.Table.FirstOrDefault(x => x.ID == ViewModel.SelectedRecepient.TableId)!.Active = true;

            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                restaurantContext.Tables.FirstOrDefault(x => x.ID == ViewModel.SelectedRecepient.TableId)!.Active = true;
                restaurantContext.SaveChanges();
            }

            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                foreach (var item in collection)
                    restaurantContext.ProductsOrders.Add(new ProductOrder() { OrderId = order.ID, ProductId = item });
                restaurantContext.Orders.FirstOrDefault(x => x.ID == ViewModel.SelectedRecepient.OrderID).Active = true;
                restaurantContext.SaveChanges();
            }


            ViewModel.RemoveProductOrderToId(order.ID);
            ViewModel.RemoveInOrders(order);
            ViewModel.RemoveCheck(ViewModel.SelectedRecepient);

            SendMessage(new LogicClassToProducts { Function = "$SENDMESSAGE_TO_CLIENT", RecepietId = order.ID, Products = collection });
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #region ViewModel to DB
        private void ComboBoxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBoxProductsFromMenu.Items.Clear();
                Category sel = (Category)ComboBoxCategories.SelectedValue;
                Category selectedCategory = ViewModel.Category.FirstOrDefault(a => a.Name == sel.Name)!;
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
            CreateOrder();
        }
        //private void ComboBoxTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        ListBoxProductsFromOrderByTableNumber.Items.Clear();
        //        Data_Access_Entity.Entities.Table selecTable = (Data_Access_Entity.Entities.Table)ComboBoxTables.SelectedValue;
        //        Order thisorder = ViewModel.Orders.FirstOrDefault(o => o.Active == false && o.TableId == selecTable.ID)!;
        //        if (thisorder != null)
        //        {
        //            var b = ViewModel.GetProductId(thisorder.ID);
        //            List<Product> Show = new List<Product>();
        //            foreach (var item in b)
        //            {
        //                Show.Add(ViewModel.Product.FirstOrDefault(x => x.ID == item)!);
        //            }
        //            foreach (var item in Show)
        //                ListBoxProductsFromOrderByTableNumber.Items.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        private void GetOrderItems()
        {
            ListBoxProductsFromOrderByTableNumber.Items.Clear();
            Data_Access_Entity.Entities.Table selecTable = (Data_Access_Entity.Entities.Table)ListBoxTables.SelectedValue;
            Order thisorder = ViewModel.Orders.FirstOrDefault(o => o.Active == false && o.TableId == selecTable.ID)!;
            if (thisorder != null)
            {
                var b = ViewModel.GetProductId(thisorder.ID);
                List<Product> Show = new List<Product>();
                foreach (var item in b)
                {
                    Show.Add(ViewModel.Product.FirstOrDefault(x => x.ID == item)!);
                }
                foreach (var item in Show)
                    ListBoxProductsFromOrderByTableNumber.Items.Add(item);
            }
        }
        private void GetProducts()
        {
            try
            {
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    var products = restaurantContext.Products;
                    foreach (var item in products)
                    {
                        ViewModel.AddInProduct(item);
                    }
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
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    var categories = restaurantContext.Categories;
                    foreach (var item in categories)
                    {
                        ViewModel.AddInCategory(item);
                    }
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
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    var tables = restaurantContext.Tables;
                    foreach (var item in tables)
                    {
                        if (item.WaiterId == User.ID)
                            ViewModel.AddInTable(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetWaiter()
        {
            try
            {
                using (RestaurantContext restaurantContext = new RestaurantContext())
                {
                    var waiter = restaurantContext.Waiters;
                    foreach (var item in waiter)
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
        private void DbToViewModel()
        {
            GetCategories();
            GetProducts();
            GetTables();
            GetWaiter();
        }
        private void ListBoxTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxProductsFromOrderByTableNumber.Items.Clear();
            Data_Access_Entity.Entities.Table selecTable = (Data_Access_Entity.Entities.Table)ListBoxTables.SelectedValue;
            Order thisorder = ViewModel.Orders.FirstOrDefault(o => o.Active == false && o.TableId == selecTable.ID)!;
            if (thisorder != null)
            {
                var b = ViewModel.GetProductId(thisorder.ID);
                List<Product> Show = new List<Product>();
                foreach (var item in b)
                {
                    Show.Add(ViewModel.Product.FirstOrDefault(x => x.ID == item)!);
                }
                foreach (var item in Show)
                    ListBoxProductsFromOrderByTableNumber.Items.Add(item);
            }
        }

        private void ListBoxProductsFromMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CreateOrder();
        }
        private void CreateOrder()
        {
            try
            {
                if (ListBoxTables.SelectedValue != null && ListBoxProductsFromMenu.SelectedItem != null)
                {
                    Data_Access_Entity.Entities.Table selTable = (Data_Access_Entity.Entities.Table)ListBoxTables.SelectedValue;
                    Order thisorder = ViewModel.Orders.FirstOrDefault(o => o.Active == false && o.TableId == selTable.ID)!;
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
                        using (RestaurantContext restaurantContext = new RestaurantContext())
                        {
                            var time = DateTime.Now;
                            restaurantContext.Orders.Add(new Order
                            {
                                Active = false,
                                OrderDate = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0),
                                TableId = selTable.ID,
                                WaiterId = User.ID
                            });
                            restaurantContext.SaveChanges();
                            var newOrder = restaurantContext.Orders.FirstOrDefault(o => o.Active == false && o.TableId == selTable.ID);
                            ViewModel.AddInOrders(new Order
                            {
                                ID = newOrder.ID,
                                Active = false,
                                OrderDate = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute,0),
                                TableId = selTable.ID,
                                WaiterId = User.ID
                            });
                            ViewModel.AddInProductOrder(new ProductOrder
                            {
                                OrderId = newOrder.ID,
                                ProductId = selectedvalue.ID
                            });
                        }
                    }
                    ViewModel.Table.FirstOrDefault(o => o.ID == selTable.ID)!.Active = false;
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
        #endregion

        private void ReserveTable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnreserveTable_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
