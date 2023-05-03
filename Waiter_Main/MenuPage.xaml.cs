using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Waiter_App;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using LibraryForServer;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Data_Access_Entity.Entities;
using Waiter_App.ViewModel_Models;

namespace Waiter_Main
{

    public partial class MainWindow : Window
    {
        static int WaiterID;
        IPEndPoint serverEndPoint;
        UdpClient client;

        ViewModel viewModel = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
        public MainWindow(int Id) : this()
        {
            WaiterID = Id;
            #region Connect to server
            client = new UdpClient();
            string serverAddress = ConfigurationManager.AppSettings["ServerAddress"]!;
            short serverPort = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
            SendMessage(new LogicClassToRecipient { Function = "$WAITERJOIN",Id = WaiterID});
            ListenAsync();
            #endregion

            DataContext = viewModel;


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
            Login login = new Login();
            this.Close();
            login.ShowDialog();
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }


        #endregion

        private void ActiveOrderLBItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Orders orders = new Orders();
            orders.Show();
        }

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
                        Order order = classToOrders.Order;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            viewModel.AddInNew(new StringClass() { Message = classToOrders.Msg });
                            if(order != null) 
                                MessageBox.Show($"ID : {order.ID}\nWaiter ID : {order.WaiterId}\nDate : {order.OrderDate}\nActive : {order.Active}\n Message : {classToOrders.Msg}");
                        });
                    }
                    else if (logic.Function == "$SENDMESSAGE_TO_WAITER")
                    {
                        LogicClassToCheck logicClassToCheck = (LogicClassToCheck)logic;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            viewModel.AddInReceipts(new IDClass { TableId = logicClassToCheck.TableID, OrderID = logicClassToCheck.OrderId });
                        });
                        //Після чого можна було б зробити DOUBLE CLICK на ListBox повідомлення про чек, і воно надсилає чек нашому клієнту по полю logicClassToCheck.RecepientId
                        //перед цим це поле - logicClassToCheck.RecepientId,  труба було б змінити на Order.Id
                    }
                    else if (logic.Function == "$SENDMESSAGE")
                    {
                        LogicClassToMessage logicClassToMessage = (LogicClassToMessage)logic;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            viewModel.AddInNew(new StringClass { Message = logicClassToMessage.Message });
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

        private void ListBoxCheck_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var Table = viewModel.SelectedRecepient.TableId;

            Order order = new Order() { ID = 10, Active = true, OrderDate = DateTime.Now, WaiterId = 1 };
            SendMessage(new LogicClassToCheck { Function = "$SENDMESSAGE_TO_CLIENT", RecipientId = order.ID, Order = order});

        }
    }
}
