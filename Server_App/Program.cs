
using LibraryForServer;
using Server_App;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

RestaurantServer restaurantServer = new RestaurantServer();
restaurantServer.Start();
class RestaurantServer
{
    const short port = 4040;
    HashSet<WaitersIp> Waiters = new HashSet<WaitersIp>();
    UdpClient server = new UdpClient(port);
    IPEndPoint clientIPEndPoint = null;
    private void AddWaiter(IPEndPoint memberm, int id)
    {
        Waiters.Add(new WaitersIp { Id = id, Ip = memberm });
        Console.WriteLine($@" {clientIPEndPoint} Waiter was added, Id : {id}");
    }
    private LogicClass ConvertFromBytes(byte[] data)
    {

        using (var memStream = new MemoryStream())
        {
            try
            {
                var binForm = new BinaryFormatter();
                memStream.Write(data, 0, data.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                LogicClass obj = (LogicClass)binForm.Deserialize(memStream);
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
        Console.WriteLine("Server started");
        while (true)
        {
            byte[] data = server.Receive(ref clientIPEndPoint);
            LogicClass waiter = ConvertFromBytes(data);

            if (waiter.Function == "$JOIN")
            {
                AddWaiter(clientIPEndPoint, waiter.Id);
            }

        }
    }
}
