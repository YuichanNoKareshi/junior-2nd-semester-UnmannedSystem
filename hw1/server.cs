using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class server : MonoBehaviour
{
    int 端口号 = 6000;
    string 协议号 = "127.0.0.1";
    Thread 临时线程;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("开始");
        try
        {
            Socket 接收套接字 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //turn it into big-endian
            IPAddress 网际互连协议 = IPAddress.Parse(协议号);
            IPEndPoint 端点 = new IPEndPoint(网际互连协议, 端口号);
            接收套接字.Bind(端点);
            Debug.Log("成功绑定端口 " + 端口号);
            接收套接字.Listen(10);
            临时线程 = new Thread(Listen);
            临时线程.IsBackground = true;
            临时线程.Start(接收套接字);
        }
        catch { }
    }
    
    Socket 发送套接字;

    void Listen(object 对象)
    {
        try
        {
            Socket 接收套接字 = 对象 as Socket;
            while (true)
            {
                发送套接字 = 接收套接字.Accept();
                Debug.Log(发送套接字.RemoteEndPoint.ToString() + ":" + "连接成功!");
                Thread 新线程 = new Thread(Received);
                新线程.IsBackground = true;
                新线程.Start(发送套接字);
            }
        }
        catch { }
    }

    void Received(object 对象)
    {
        try
        {
            Socket 发送套接字 = 对象 as Socket;
            while (true)
            {
                byte[] 缓冲区 = new byte[1024 * 1024 * 3];
                int 长度 = 发送套接字.Receive(缓冲区);
                if (长度 == 0) break;
                string 字符串 = Encoding.UTF8.GetString(缓冲区, 0, 长度);
                print(字符串);
                Send(字符串);
            }
        }
        catch { }
    }
    void Send(string 字符串)
    {
        byte[] 缓冲区 = Encoding.UTF8.GetBytes(字符串);
        发送套接字.Send(缓冲区);
    }
    // Update is called once per frame
    void Update()
    {
    }
    void OnApplicationQuit()
    {
        if (临时线程 != null)
        {
            临时线程.Interrupt();
            临时线程.Abort();
        }
        if (发送套接字 != null) 发送套接字.Close();
        Debug.Log("断开连接");
    }
}
