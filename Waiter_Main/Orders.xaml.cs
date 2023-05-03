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

namespace Waiter_App
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        ViewModel ViewModel = new ViewModel();
        RestaurantContext restaurantContext = new RestaurantContext();
        static int WaiterID;
        IPEndPoint serverEndPoint;
        UdpClient client;
        public Orders()
        {
            InitializeComponent();
            DbToViewModel();
            GetCategoriesToComboBox();
            GetTablesToComboBox();
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
        public Orders(int Id) : this()
        {
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
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ViewModel.AddInNew(new StringClass() { Message = classToOrders.Msg });
                            ViewModel.AddInOrders(classToOrders.order);
                        });
                        foreach (var item in classToOrders.products)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                //MessageBox.Show(item.OrderId.ToString() + " : " + item.ProductId.ToString());
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
                        //Після чого можна було б зробити DOUBLE CLICK на ListBox повідомлення про чек, і воно надсилає чек нашому клієнту по полю logicClassToCheck.RecepientId
                        //перед цим це поле - logicClassToCheck.RecepientId,  труба було б змінити на Order.Id
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

        private void ActiveOrderLBItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Orders orders = new Orders();
            orders.Show();
        }
        private void ListBoxCheck_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var Table = ViewModel.SelectedRecepient.TableId;

            Order order = new Order() { ID = 10, Active = true, OrderDate = DateTime.Now, WaiterId = 1 };
            SendMessage(new LogicClassToCheck { Function = "$SENDMESSAGE_TO_CLIENT", RecipientId = order.ID, Order = order });

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
