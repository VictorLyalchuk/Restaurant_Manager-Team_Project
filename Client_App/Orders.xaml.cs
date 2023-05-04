using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.Configuration;
using LibraryForServer;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Data_Access_Entity.Entities;
using Data_Access_Entity;

namespace Client_App
{
    public partial class Orders : Window
    {
        ViewModel ViewModel = new ViewModel();
        RestaurantContext restaurantContext = new RestaurantContext();
        IPEndPoint serverEndPoint;
        UdpClient client;
        public Orders()
        {
            InitializeComponent();
            DbToViewModel();
            GetTablesToComboBox();
            DataContext = ViewModel;

            #region Connect to server
            client = new UdpClient();
            string serverAddress = ConfigurationManager.AppSettings["ServerAddress"]!;
            short serverPort = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
            //client.Client.Bind(serverEndPoint);
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
            Application.Current.Shutdown();
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
            Menu menu = new Menu();
            this.Close();
            menu.ShowDialog();
        }
        private void Order_Click(object sender, RoutedEventArgs e)
        {
            var newOrder = restaurantContext.Orders.FirstOrDefault(o => o.Active == false && o.TableId == (int)ComboBoxTables.SelectedValue);
            restaurantContext.Tables.FirstOrDefault(x => x.ID == TableId.tableId).Active = false;
            restaurantContext.SaveChanges();
            SendMessage(new LogicClassToOrders { Function = "$ADDORDER", products = ViewModel.ProductOrder, Msg = $"• Client at table {TableId.tableId} made order", RecepientId = TableId.RecepientId, order = newOrder });
        }
        #region Function For Server
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
                    LogicClass message = (LogicClass)ConvertFromBytes(data);

                    if (message.Function == "$SENDMESSAGE_TO_CLIENT")
                    {
                        LogicClassToCheck logic = (LogicClassToCheck)message;
                        MessageBox.Show($"-----------------YOUR CHECK-----------------\nOrder ID : {logic.Order.ID}\nWaiter ID : {logic.Order.WaiterId}\nOrderDate : {logic.Order.OrderDate}");

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
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            //В поле ID - Записується Order.Id
            SendMessage(new LogicClassToRecipient { Function = "$CLIENTJOIN", Id = 10 });
            ListenAsync();

            SendMessage(new LogicClassToCheck { Function = "$SENDMESSAGE_TO_WAITER", TableID = 1, RecipientId = 1, OrderId = 10 });

        }
        #region ViewModel to BD
        private void ComboBoxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBoxProductsFromMenu.Items.Clear();
                Category selCat = (Category)ComboBoxCategories.SelectedValue;
                Category selectedCategory = ViewModel.Category.FirstOrDefault(a => a.Name == selCat.Name)!;
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
                            WaiterId = TableId.RecepientId
                        });
                        restaurantContext.SaveChanges();
                        var newOrder = restaurantContext.Orders.FirstOrDefault(o => o.Active == false && o.TableId == (int)ComboBoxTables.SelectedValue);
                        ViewModel.AddInOrders(new Order
                        {
                            ID = newOrder.ID,
                            Active = false,
                            OrderDate = DateTime.Now,
                            TableId = (int)ComboBoxTables.SelectedValue,
                            WaiterId = TableId.RecepientId
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
        private void GetTablesToComboBox()
        {
            try
            {
                ComboBoxTables.Items.Add(TableId.tableId);
                ComboBoxTables.SelectedValue = TableId.tableId;
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
        private void DbToViewModel()
        {
            GetCategories();
            GetProducts();
        }
        #endregion
    }
}
