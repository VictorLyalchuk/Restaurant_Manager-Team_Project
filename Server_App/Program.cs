
using Data_Access_Entity.Entities;
using LibraryForServer;
using Server_App;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

RestaurantServer restaurantServer = new RestaurantServer();
restaurantServer.Start();

class RestaurantServer
{
    const short port = 4040;
    HashSet<RecipientIp> Waiters = new HashSet<RecipientIp>();
    HashSet<RecipientIp> Clients = new HashSet<RecipientIp>();
    UdpClient server = new UdpClient(port);
    IPEndPoint clientIPEndPoint = null;
    private void AddWaiter(IPEndPoint memberm, int id)
    {
        Waiters.Add(new RecipientIp { Id = id, Ip = memberm });
        Console.WriteLine($@" {clientIPEndPoint} Waiter was added, Id : {id}");
    }
    private void AddClient(IPEndPoint memberm, int id)
    {
        Clients.Add(new RecipientIp { Id = id, Ip = memberm });
        Console.WriteLine($@" {clientIPEndPoint} Client was added, Id : {id}");
    }
    private async void SendMessage(object message, IPEndPoint waiter)
    {
        byte[] data;
        BinaryFormatter formatter = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            formatter.Serialize(ms, message);
            data = ms.ToArray();
        }
        await server.SendAsync(data, data.Length, waiter);
    }
    private object ConvertFromBytes(byte[] data)
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
    public void Start()
    {
        try
        {
            Console.WriteLine("Server started");
            while (true)
            {
                byte[] data = server.Receive(ref clientIPEndPoint);
                LogicClass waiter = (LogicClass)ConvertFromBytes(data);

                if (waiter.Function == "$WAITERJOIN")
                {
                    LogicClassToRecipient logic = (LogicClassToRecipient)waiter;
                    AddWaiter(clientIPEndPoint, logic.Id);
                }
                else if (waiter.Function == "$CLIENTJOIN")
                {
                    LogicClassToRecipient logic = (LogicClassToRecipient)waiter;
                    AddClient(clientIPEndPoint, logic.Id);
                }
                else if (waiter.Function == "$ADDORDER")
                {
                    LogicClassToOrders logic = (LogicClassToOrders)waiter;
                    //Console.WriteLine(logic.Order.ID.ToString() + " " + logic.Order.OrderDate);
                    Order order = logic.Order;
                    IPEndPoint CuurentWaiter = null;
                    foreach (var item in Waiters)
                        if (item.Id == order.WaiterId)
                            CuurentWaiter = item.Ip;
                    SendMessage(new LogicClassToOrders { Function = "$ADDORDER", Order = order, Msg = logic.Msg }, CuurentWaiter!);
                }
                else if (waiter.Function == "$SENDMESSAGE_TO_WAITER")
                {
                    LogicClassToCheck logic = (LogicClassToCheck)waiter;
                    //Console.WriteLine(logic.Message);
                    IPEndPoint CuurentWaiter = null;
                    foreach (var item in Waiters)
                        if (item.Id == logic.RecipientId)
                            CuurentWaiter = item.Ip;
                    SendMessage(logic, CuurentWaiter!);
                }
                else if (waiter.Function == "$SENDMESSAGE_TO_CLIENT")
                {
                    LogicClassToCheck logic = (LogicClassToCheck)waiter;
                    //Console.WriteLine("Object was sended to CLIENT with ID : " + logic.RecipientId);
                    IPEndPoint CuurentClient = null;
                    foreach (var item in Clients)
                        if (item.Id == logic.RecipientId)
                            CuurentClient = item.Ip;
                    SendMessage(logic, CuurentClient!);
                }
                else if (waiter.Function == "$SENDMESSAGE")
                {
                    LogicClassToMessage logic = (LogicClassToMessage)waiter;
                    IPEndPoint CuurentClient = null;
                    foreach (var item in Waiters)
                        if (item.Id == logic.RecipientId)
                            CuurentClient = item.Ip;
                    SendMessage(logic, CuurentClient!);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
