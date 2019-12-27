using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

public class Client
{
    private int m_port;
    private string m_ip;

    public Client(int port, string ip)
    {
        m_port = port;
        m_ip = ip;
    }

    public bool testConnection()
    {
        bool connected = false;
        TcpClient client = new TcpClient(m_ip, m_port);
        if (client.Connected)
            connected = true;

        client.Close();

        return connected;
    }

    public string sendMessage(string message)
    {
        if (message.Length == 0)
            return "No meesage to send";

        TcpClient client = new TcpClient(m_ip, m_port);
        NetworkStream netStream = client.GetStream();

        byte[] bytes = ASCIIEncoding.ASCII.GetBytes(message);

        netStream.Write(bytes, 0, bytes.Length);

        byte[] recievedBytes = new byte[client.ReceiveBufferSize];
        int bytesRead = netStream.Read(recievedBytes, 0, client.ReceiveBufferSize);


        return Encoding.ASCII.GetString(recievedBytes, 0, bytesRead);
    }
}