using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class NetManager
{
    public static Socket listenfd;
    public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();
    public static Dictionary<string , Player> PlayerDic = new Dictionary<string, Player>();
    public static Dictionary<string , Room> RoomDic = new Dictionary<string, Room>();
    public static List<ClientState> PlayerList = new List<ClientState>(); 
    static List<Socket> checkRead = new List<Socket>();

    public static void StartLoop(int listenPort)
    {
        listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipep = new IPEndPoint(ipAdr, listenPort);
        listenfd.Bind(ipep);
        listenfd.Listen(0);
        Console.WriteLine("服务器启动");
        while (true)
        {
            ResetCheckRead();
            Socket.Select(checkRead, null, null, 1000);
            for (int i = checkRead.Count - 1; i >= 0; i--)
            {
                Socket s = checkRead[i];
                if (s == listenfd)
                {
                    ReadListenfd(s);
                }
                else
                {
                    ReadClientfd(s);
                }
            }
        }

    }

    public static void ResetCheckRead()
    {
        checkRead.Clear();
        checkRead.Add(listenfd);
        foreach (ClientState item in clients.Values)
        {
            checkRead.Add(item.socket);
        }
    }

    public static void ReadListenfd(Socket listenfd)
    {
        try
        {
            Socket clientfd = listenfd.Accept();
            Console.WriteLine("Accept" + clientfd.RemoteEndPoint.ToString());
            ClientState state = new ClientState();
            state.socket = clientfd;
            clients.Add(clientfd, state);

        }
        catch (SocketException ex)
        {
            Console.WriteLine("Accept fail" + ex.ToString());
        }
    }

    public static void ReadClientfd(Socket clientfd)
    {
        ClientState state = clients[clientfd];
        ByteArray readBuff = state.readBuff;
        int count = 0;
        if (readBuff.remain <= 0)
        {
            OnReceiveData(state);
            readBuff.MoveBytes();

        }
        if (readBuff.remain <= 0)
        {
            Console.WriteLine("Receive fail,maybe msg length>buff capacity");
            Close(state);
            return;
        }
        try
        {
            count = clientfd.Receive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0);

        }
        catch (SocketException ex)
        {
            Console.WriteLine("Receive SocketEx" + ex.ToString());
            Close(state);
            return;
        }
        if (count <= 0)
        {
            Console.WriteLine("Socket Close" + clientfd.RemoteEndPoint.ToString());
            Close(state);
            return;

        }
        readBuff.writeIdx += count;
        OnReceiveData(state);
        readBuff.CheckAndMoveBytes();
    }

    public static void Close(ClientState state)
    {
        MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
        object[] ob = { state };
        mei.Invoke(null, ob);
        state.socket.Close();
        clients.Remove(state.socket);
    }

    public static void OnReceiveData(ClientState state)
    {
        ByteArray readBuff = state.readBuff;
        byte[] bytes = readBuff.bytes;
        if (readBuff.length < 2)
        {
            return;
        }
        Int16 totalLength = (Int16)(bytes[readBuff.readIdx + 1] << 8 | bytes[readBuff.readIdx]);
        if (readBuff.length < totalLength)
        {
            return;
        }
        readBuff.readIdx += 2;
        int nameCount = 0;
        string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out nameCount);
        if (protoName == "")
        {
            Console.WriteLine("OnReceiveData MsgBase.DecodeName fail");
            Close(state);

        }
        readBuff.readIdx += nameCount;
        int bodyCount = totalLength - nameCount;
        MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);
        readBuff.readIdx += bodyCount;
        readBuff.CheckAndMoveBytes();
        MethodInfo mi = typeof(MsgHandler).GetMethod(protoName);
        object[] o = { state, msgBase };
        Console.WriteLine("Receive" + protoName);
        if (mi != null)
        {
            mi.Invoke(null, o);
        }
        else
        {
            Console.WriteLine("OnReceiveData invoke fail" + protoName);

        }
        if (readBuff.length > 2)
        {
            OnReceiveData(state);
        }
    }
    public static void Send(Socket socket, MsgBase msg)
    {
        if (socket == null)
        {
            return;
        }
        if (! socket.Connected)
        {
            return;
        }
        byte[] nameBytes = MsgBase.EncodeName(msg);
        byte[] bodyBytes = MsgBase.Encode(msg);
        int length = nameBytes.Length + bodyBytes.Length;
        byte[] sendBytes = new byte[2 + length];
        sendBytes[0] = (byte)(length % 256);
        sendBytes[1] = (byte)(length / 256);
        Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
        Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);

        try
        {

            socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, null);
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Socket Close on BeginSend" + ex.ToString());
        }

    }
    public static void Send(ClientState cs, MsgBase msg)
    {
        if (cs == null)
        {
            return;
        }
        if (!cs.socket.Connected)
        {
            return;
        }
        byte[] nameBytes = MsgBase.EncodeName(msg);
        byte[] bodyBytes = MsgBase.Encode(msg);
        int length = nameBytes.Length + bodyBytes.Length;
        byte[] sendBytes = new byte[2 + length];
        sendBytes[0] = (byte)(length % 256);
        sendBytes[1] = (byte)(length / 256);
        Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
        Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);

        try
        {

            cs.socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, null);
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Socket Close on BeginSend" + ex.ToString());
        }

    }
}
