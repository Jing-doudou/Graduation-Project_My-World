using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading; 
using UnityEngine;

public enum NetEvent { ConnectSucc = 1, ConnectFail = 2, Close = 3 }//事件

public class NetManager
{
    static Socket socket;
    static ByteArray readBuff;
    static Queue<ByteArray> writeQueue;
    static bool isConnecting = false;
    static bool isClosing = false;


    public delegate void EventListener(string err);
    static Dictionary<NetEvent, EventListener> eventListeners =
        new Dictionary<NetEvent, EventListener>();
    public delegate void MsgListener(MsgBase msgBase);
    static Dictionary<string, MsgListener> msgListeners =
        new Dictionary<string, MsgListener>();
    static List<MsgBase> msgList = new List<MsgBase>();
    static int msgCount = 0;
    readonly static int MAX_MESSAGE_FIRE = 10;

    public static bool isUsePing = true; 


    public static void AddEventListener(NetEvent netEvent, EventListener listener)//添加事件监听
    {
        if (eventListeners.ContainsKey(netEvent))//添加事件
        {
            eventListeners[netEvent] += listener;
        }
        else//新增事件
        {
            eventListeners[netEvent] = listener;
        }
    }
    public static void RemoveEventListener(NetEvent netEvent, EventListener listener)//删除事件监听
    {
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent] -= listener;
            if (eventListeners[netEvent] == null)
            {
                eventListeners.Remove(netEvent);
            }
        }
    }

    public static void FireEvent(NetEvent netEvent, string err)
    {
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent](err);
        }
    }
    public static void AddMsgListener(string msgName, MsgListener listener)
    {
        if (msgListeners.ContainsKey(msgName))
        {
            msgListeners[msgName] += listener;

        }
        else
        {
            msgListeners[msgName] = listener;
        }
    }

    public static void RemoveMsgListener(string msgName, MsgListener listener)
    {
        if (msgListeners.ContainsKey(msgName))
        {
            msgListeners[msgName] -= listener;
            if (msgListeners[msgName] == null)
            {
                msgListeners.Remove(msgName);
            }
        }
    }
    public static void FireMsg(string msgName, MsgBase msg)
    {
        if (msgListeners.ContainsKey(msgName))
        {
            msgListeners[msgName](msg);
        }
    }
    public static void Connect(string ip, int port)
    {
        if (socket != null && socket.Connected)
        {
            Debug.Log("Connect fail,already connected");
            return;
        }
        if (isConnecting)
        {
            Debug.Log("Connect fail,isConnecting");
            return;
        }
        InitState();
        socket.NoDelay = true;
        socket.BeginConnect(ip, port, ConnectCallback, socket);
    }

    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket connect succ");
            FireEvent(NetEvent.ConnectSucc, "");
            isConnecting = false;
            socket.BeginReceive(
               readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket connect fail" + ex.ToString());
            FireEvent(NetEvent.ConnectFail, ex.ToString());
            isConnecting = false;

        }

    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndReceive(ar);
            if (count == 0)
            {
                Close();
                return;
            }
            readBuff.writeIdx += count;
            OnReceiveData();
            if (readBuff.remain < 8)
            {
                readBuff.MoveBytes();
                readBuff.Resize(readBuff.length * 2);
            }
            socket.BeginReceive(
                readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Receive fail" + ex.ToString());
        }
    }

    private static void OnReceiveData()
    {
        if (readBuff.length <= 2)
        {
            return;
        }
        int readIdx = readBuff.readIdx;
        byte[] bytes = readBuff.bytes;
        Int16 totalLength = (Int16)(bytes[readIdx + 1] << 8 | bytes[readIdx]);
        if (readBuff.length < totalLength + 2)
        {
            return;
        }
        readBuff.readIdx += 2;
        int nameCount = 0;
        string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out nameCount);
        if (protoName == "")
        {
            Debug.Log("OnreceiveData Decodename fail");
            return;
        }
        readBuff.readIdx+=nameCount;
        //
        int bodyCount = totalLength - nameCount;
        
        MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);
        readBuff.readIdx += bodyCount;
        //
        readBuff.CheckAndMoveBytes();
        
        lock (msgList)
        {
            msgList.Add(msgBase);
        }
        msgCount++;
        
        if (readBuff.length > 2)
        {
            OnReceiveData();
        }
    }

    static void InitState()
    {
        socket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        readBuff = new ByteArray();
        writeQueue = new Queue<ByteArray>();
        isConnecting = false;
        isClosing = false;
        msgList = new List<MsgBase>();
        msgCount = 0; 
    }

    public static void Close()
    {
        if (socket == null || !socket.Connected)
        {
            return;
        }
        if (isConnecting)
        {
            return;
        }
        if (writeQueue.Count > 0)
        {
            isClosing = true;
        }
        else
        {
            socket.Close();
            FireEvent(NetEvent.Close, "");
        }
    }


    public static void Send(MsgBase msgBase)
    {
        if (socket == null || !socket.Connected)
        {
            return;
        }
        if (isConnecting)
        {
            return;
        }
        if (isClosing)
        {
            return;
        }
        byte[] nameBytes = MsgBase.EncodeName(msgBase);
        byte[] bodyBytes = MsgBase.Encode(msgBase);
        int len = nameBytes.Length + bodyBytes.Length;
        byte[] sendBytes = new byte[2 + len];
        sendBytes[0] = (byte)(len % 256);
        sendBytes[1] = (byte)(len / 256);
        Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
        Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
        ByteArray ba = new ByteArray(sendBytes);
        int count = 0;
        lock (writeQueue)
        {
            writeQueue.Enqueue(ba);
            count = writeQueue.Count;
        }
        if (count == 1)
        {
            socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        Socket socket = (Socket)ar.AsyncState;
        if (socket == null || !socket.Connected)
        {
            return;
        }
        int count = socket.EndSend(ar);
        ByteArray ba;
        lock (writeQueue)
        {
            ba = writeQueue.First();
        }
        ba.readIdx += count;
        if (ba.length == 0)
        {
            lock (writeQueue)
            {
                writeQueue.Dequeue();
                ba = writeQueue.First();
            }
        }
        if (ba != null)
        {
            socket.BeginSend(ba.bytes, ba.readIdx, ba.length, 0, SendCallback, socket);
        }
        else if (isClosing)
        {
            socket.Close();
        }

    }

    public static void Update()
    {
        MsgUpdate(); 
    }
    private static void MsgUpdate()
    {
        if (msgCount == 0)
        {
            return;
        }
        for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
        {
            MsgBase msgBase = null;
            lock (msgList)
            {
                if (msgList.Count > 0)
                {
                   // Debug.Log(222);
                    msgBase = msgList[0];
                    msgList.RemoveAt(0);
                    msgCount--;
                }
                if (msgBase != null)
                {
                    //Debug.Log(111);
                    FireMsg(msgBase.protoName, msgBase);
                }
                else
                {
                    break;
                }
            }
        }
    } 
     
}
