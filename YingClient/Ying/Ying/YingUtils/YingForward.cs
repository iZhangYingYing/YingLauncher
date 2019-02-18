using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ying.YingUtil
{

    /// <summary>
    /// user => tcp1 => tcp2
    /// </summary>
    class YingForward
    {
        int localProt { get; set; }
        string localIp { get; set; }
        int TargetPort { get; set; }
        string TargetIp { get; set; }
        public YingForward(string localIp, int localProt, string TargetIp, int TargetPort)
        {
            this.localIp = localIp;
            this.localProt = localProt;
            this.TargetIp = TargetIp;
            this.TargetPort = TargetPort;
        }

        public YingForward()
        {
            this.localIp = "0.0.0.0";
            this.localProt = 8080;
            this.TargetIp = "0.0.0.0";
            this.TargetPort = 80;
        }

        public void Run()
        {
            //服务器IP地址  
            IPAddress ip = IPAddress.Parse(localIp);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); ;
            serverSocket.Bind(new IPEndPoint(ip, localProt));
            serverSocket.Listen(10000);
            Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            Thread myThread = new Thread(Listen);
            myThread.Start(serverSocket);
        }

        //监听客户端连接
        private void Listen(object obj)
        {
            Socket serverSocket = (Socket)obj;
            IPAddress ip = IPAddress.Parse(TargetIp);
            while (true)
            {
                Socket tcp1 = serverSocket.Accept();
                Socket tcp2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcp2.Connect(new IPEndPoint(ip, TargetPort));
                //目标主机返回数据
                ThreadPool.QueueUserWorkItem(new WaitCallback(SwapMsg), new thSock
                {
                    tcp1 = tcp2,
                    tcp2 = tcp1
                });
                //中间主机请求数据
                ThreadPool.QueueUserWorkItem(new WaitCallback(SwapMsg), new thSock
                {
                    tcp1 = tcp1,
                    tcp2 = tcp2
                });
            }
        }
        ///两个 tcp 连接 交换数据，一发一收
        public void SwapMsg(object obj)
        {
            thSock mSocket = (thSock)obj;
            while (true)
            {
                try
                {
                    byte[] result = new byte[1024];
                    int num = mSocket.tcp2.Receive(result, result.Length, SocketFlags.None);
                    if (num == 0) //接受空包关闭连接
                    {
                        if (mSocket.tcp1.Connected)
                        {
                            mSocket.tcp1.Close();
                        }
                        if (mSocket.tcp2.Connected)
                        {
                            mSocket.tcp2.Close();
                        }
                        break;
                    }
                    mSocket.tcp1.Send(result, num, SocketFlags.None);
                    //if(mSocket.tcp1.AddressFamily)
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (mSocket.tcp1.Connected)
                    {
                        mSocket.tcp1.Close();
                    }
                    if (mSocket.tcp2.Connected)
                    {
                        mSocket.tcp2.Close();
                    }
                    break;
                }
            }
        }

    }

    public class thSock
    {
        public Socket tcp1 { get; set; }
        public Socket tcp2 { get; set; }
    }
}
