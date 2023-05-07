using Data_Access_Entity;
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Data_Access_Entity.Entities;
using LibraryForServer;
using Table = Data_Access_Entity.Entities.Table;

namespace Client_App
{
    public partial class Menu : Window
    {

        IPEndPoint serverEndPoint;
        UdpClient client;

        public Menu()
        {
            InitializeComponent();
            GenerateTable();
            #region Connect to server
            client = new UdpClient();
            string serverAddress = ConfigurationManager.AppSettings["ServerAddress"]!;
            short serverPort = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
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
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                restaurantContext.Tables.FirstOrDefault(x => x.ID == TableId.tableId).Active = true;
                restaurantContext.SaveChanges();
            }
            MainWindow login = new MainWindow();
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

        private void OpenCatalogueBtn_Click(object sender, RoutedEventArgs e)
        {
            Orders order = new Orders();
            this.Close();
            order.ShowDialog();
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
        #endregion

        private void ServeMe_Click(object sender, RoutedEventArgs e)
        {
            if(TableId.tableId == 0)
            {
                MessageBox.Show("Sorry, all seats are taken");
                return;
            };
            SendMessage(new LogicClassToMessage() { Function = "$SENDMESSAGE", RecipientId = TableId.RecepientId, Message = $"• Client at table {TableId.tableId} needs waiter" });
        }
        private void GenerateTable()
        {
            if (TableId.tableId > 0) return;
            List<Table> Tables;
            using (RestaurantContext restaurantContext = new RestaurantContext())
            {
                Tables = restaurantContext.Tables.ToList();
            }

            foreach (var item in Tables)
            {
                if (item.Active == true)
                {
                    TableId.tableId = item.ID;
                    TableId.RecepientId = item.WaiterId;
                    using (RestaurantContext restaurantContext = new RestaurantContext())
                    {
                        restaurantContext.Tables.FirstOrDefault(x => x.ID == TableId.tableId).Active = false;
                        restaurantContext.SaveChanges();
                    }
                    break;
                }
            }
        }

        private void StaffRating_Click(object sender, RoutedEventArgs e)
        {
            StaffRating rating = new StaffRating();
            this.Close();
            rating.Show();
        }
    }
}
