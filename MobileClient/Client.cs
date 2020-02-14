using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

public class Client
{
    public int m_port { get; }
    public string m_ip { get; }

    public Client(int port, string ip)
    {
        m_port = port;
        m_ip = ip;
    }

    public PingReply testConnection()
    {
        Ping ping = new Ping();
        int timeout = 120;

       return ping.Send(m_ip, timeout);
    }

    public string sendMessage(string message)
    {
        if (message.Length == 0)
            return "No meesage to send";

        TcpClient client = new TcpClient();


        if (!client.ConnectAsync(m_ip, m_port).Wait(1000))
            return $"No listeners on port {m_port}.";
        
        NetworkStream netStream = client.GetStream();

        byte[] bytes = Encoding.ASCII.GetBytes(message);

        netStream.Write(bytes, 0, bytes.Length);

        byte[] recievedBytes = new byte[client.ReceiveBufferSize];
        int bytesRead = netStream.Read(recievedBytes, 0, client.ReceiveBufferSize);

        client.Close();
        return Encoding.ASCII.GetString(recievedBytes, 0, bytesRead);
    }
}