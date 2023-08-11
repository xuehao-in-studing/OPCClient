using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Configuration;
using System.Net.NetworkInformation;
using System.ServiceModel.Channels;

namespace OPCClient
{
    enum SocketWriteSatus
    {
        READ,
        WRITE
    }

    internal class SocketClient
    {
        private Socket clientSocket;
        private string ip;
        private IPAddress address;
        private int port;
        private IPEndPoint endPoint;

        SocketWriteSatus writeSatus = SocketWriteSatus.READ;

        // 缓存管理器
        private BufferManager bufferManager;
        private static int connectionCount = 10;  // socket nums
        private int oprToPreAlloc = 2;  // allocate memory for read and write
        private int bufferSize = 256;

        SocketAsyncEventArgsPool socketAsyncEventArgsPool = new SocketAsyncEventArgsPool(connectionCount);
        SocketAsyncEventArgs SendSAE = new SocketAsyncEventArgs();
        SocketAsyncEventArgs RecieveSAE = new SocketAsyncEventArgs();


        public bool CommCycle = true;
        // flag for read and write
        // true: read false:write
        // 报文数
        private int TeleNum = 0;

        // 读取节点的数据属性
        private String[] tagTypes;
        private String[] tagNames;
        private String[] writeable;

        private bool connectFlag = false;

        public string IP { get => ip; set => ip = value; }
        public int Port { get => port; set => port = value; }
        public string[] TagNames { get => tagNames; set => tagNames = value; }
        public string[] TagTypes { get => tagTypes; set => tagTypes = value; }
        public string[] Writeable { get => writeable; set => writeable = value; }
        public IPEndPoint EndPoint { get => endPoint; set => endPoint = value; }
        public IPAddress Address { get => address; set => address = value; }

        public SocketClient()
        {
            //IP = ConfigurationManager.AppSettings.Get("ip");
            //address = IPAddress.Parse(IP);
            //Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("port"));

            //TagNames = (String[])ConfigurationManager.AppSettings.Get("m_tags").Split(';').Clone();
            //TagTypes = (String[])ConfigurationManager.AppSettings.Get("m_types").Split(';').Clone();
            //Writeable = (String[])ConfigurationManager.AppSettings.Get("writeable").Split(';').Clone();

            //bufferManager = new BufferManager(bufferSize * connectionCount * oprToPreAlloc, bufferSize);
            //bufferManager.InitBuffer();
            //Connect();
            //Thread thread1 = new Thread(new ThreadStart(Communication));
            //thread1.Start();
        }

        public void Init()
        {
            Address = IPAddress.Parse(IP);
            EndPoint = new IPEndPoint(Address, Port);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            bufferManager = new BufferManager(bufferSize * connectionCount * oprToPreAlloc, bufferSize);
            bufferManager.InitBuffer();

            Thread thread1 = new Thread(new ThreadStart(SocketCommunication));
            thread1.Start();
        }

        public void Connect()
        {
            if (Address.ToString() == "") return;
            else
            {
                Thread newthread = new Thread(ConnectToServer);
                newthread.Start();
                newthread.Join();
            }
        }

        public void ConnectToServer()
        {

            try
            {
                try
                {
                    clientSocket.Connect(EndPoint);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                if (clientSocket.Connected)
                {
                    Console.WriteLine("Connect is successful");
                }
                else if (clientSocket.Poll(0, SelectMode.SelectWrite))
                {
                    Console.WriteLine("\nThis Socket is writable.");
                }
                else if (clientSocket.Poll(0, SelectMode.SelectRead))
                {
                    Console.WriteLine("\nThis Socket is readable.");
                }
                else if (clientSocket.Poll(0, SelectMode.SelectError))
                {
                    Console.WriteLine("\nThis Socket has an error.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.ToString());
                clientSocket.Close();
            }

        }

        private void ReadPart()
        {
            Console.WriteLine("begin read");

            if (bufferManager.SetBuffer(RecieveSAE))
            {
                RecieveSAE.Completed += new EventHandler<SocketAsyncEventArgs>(RecieveSAE_Completed);
                // 这里报错，RecieveSAE 对象没有绑定好？
                clientSocket.ReceiveAsync(RecieveSAE);
            } 
            Thread.Sleep(50);
        }

        private void RecieveSAE_Completed(object sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine("读取完毕回调函数调用了");
            string msg;
            Socket sk = sender as Socket;
            byte[] data = e.Buffer;
            msg = System.Text.Encoding.UTF8.GetString(data);
            //Console.WriteLine(msg.Trim());

            //*************************************************************************
            string datavalue = msg.Substring(0, 8).Replace("\0", "").Replace("R", "").Trim();  //此段报文解析，读取的变量不同解析方式应该也不同，待测试。


            Console.WriteLine(datavalue);

            try
            {
                // 异步再调用，可以实现类似循环读取
                //e.Completed += new EventHandler<SocketAsyncEventArgs>(RecieveSAE_Completed);
                clientSocket.ReceiveAsync(e);
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }

        }

        private void SendSAE_Completed(object sender, SocketAsyncEventArgs e)
        {
            //throw new NotImplementedException();
        }
        public void SocketCommunication()
        {
            //while (CommCycle)
            {
                #region main loop
                try
                {
                    Ping pingSender = new Ping();
                    PingReply reply = pingSender.Send(IP);//第一个参数为ip地址，第二个参数为ping的时间
                    if (reply.Status == IPStatus.Success && connectFlag == false)
                    {
                        // first connected
                        Connect();
                        connectFlag = true;
                    }
                    if (reply.Status != IPStatus.Success)
                    {
                        // ping error
                        connectFlag = false;
                    }
                    if (connectFlag && !SocketConnectBad())
                    {
                        Run();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion
                Thread.Sleep(200);
            }

        }

        private void Run()
        {
            if (writeSatus == SocketWriteSatus.WRITE)
            {
                WritePart();
            }
            if (writeSatus == SocketWriteSatus.READ)
            {
                ReadPart();
            }
        }

        private void WritePart()
        {
            SocketAsyncEventArgs SendSAE = new SocketAsyncEventArgs();

            byte[] data = Encoding.UTF8.GetBytes(TagNames[TeleNum] + " \r\n");
            //SendSAE.SetBuffer(data, 0, data.Length);

            if (bufferManager.SetBuffer(SendSAE))
            {
                Console.WriteLine("SendSAE set buffer success");
            }
            else
            {
                Console.WriteLine("Set buffer error");
            }

            SendSAE.Completed += new EventHandler<SocketAsyncEventArgs>(SendSAE_Completed);

            clientSocket.SendAsync(SendSAE);
            Console.WriteLine("write once");
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 有必要测试吗，因为有一个是拒绝写入的
        /// </summary>
        /// <returns></returns>
        public bool SocketConnectBad()
        {
            bool blockingState = clientSocket.Blocking;
            try
            {
                byte[] tmp = new byte[1];
                clientSocket.Blocking = false;

                int res = clientSocket.Send(tmp);
                return false;
            }
            catch (SocketException e)
            {
                if (e.NativeErrorCode.Equals(10035))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            finally
            {
                clientSocket.Blocking = blockingState;
            }
        }

    }



}
