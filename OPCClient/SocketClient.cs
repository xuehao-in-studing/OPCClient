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

    internal class SocketClient
    {
        private Socket clientSocket;
        private string ip;
        private IPAddress address;
        private int port;
        private IPEndPoint endPoint;


        private BufferManager bufferManager;
        private int connectionCount=1;  // socket nums
        private int oprToPreAlloc = 2;  // allocate memory for read and write
        private int bufferSize = 256;

        public bool CommCycle = true;
        // flag for read and write
        // true: read false:write
        public bool WriteReadShift = true;
        // 报文数
        private int TeleNum = 0;

        // 读取节点的数据属性
        private String[] tagTypes;
        private String[] tagNames;
        private String[] writeable;

        private bool connectFlag = false;
        public SocketClient()
        {
            ip = ConfigurationManager.AppSettings.Get("ip");
            address = IPAddress.Parse(ip);
            port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("port"));

            tagNames = (String[])ConfigurationManager.AppSettings.Get("m_tags").Split(';').Clone();
            tagTypes = (String[])ConfigurationManager.AppSettings.Get("m_types").Split(';').Clone();
            writeable = (String[])ConfigurationManager.AppSettings.Get("writeable").Split(';').Clone();

            bufferManager = new BufferManager(bufferSize* connectionCount*oprToPreAlloc , bufferSize);
            bufferManager.InitBuffer();
            Connect();
            Thread thread1 = new Thread(new ThreadStart(Communication));
            thread1.Start();
        }

        public void Connect()
        {
            if (address.ToString() == "") return;
            else
            {
                Thread newthread = new Thread(ConnectToServer);
                newthread.Start();
            }
        }
        public void ConnectToServer()
        {
            endPoint = new IPEndPoint(address, port);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                try
                {
                    clientSocket.Connect(endPoint);
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
                //counter++;
                //Console.WriteLine($"{counter} client have connected");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.ToString());
                clientSocket.Close();
            }

        }

        private void ReadPart()
        {
            Console.WriteLine("read once");
            WriteReadShift = false;
            for (TeleNum = 0; TeleNum < tagNames.Length; TeleNum++)     //循环次数=XML中所组态的读报文条数  
            {
                if (tagNames[TeleNum] == " ")
                {
                    continue;
                }
                SocketAsyncEventArgs SendSAE = new SocketAsyncEventArgs();

                byte[] data = Encoding.UTF8.GetBytes(tagNames[TeleNum] + " \r\n");
                SendSAE.SetBuffer(data, 0, data.Length);

                SendSAE.Completed += new EventHandler<SocketAsyncEventArgs>(SendSAE_Completed);

                clientSocket.SendAsync(SendSAE);
                //if (bufferManager.SetBuffer(SendSAE))
                //{

                //    Console.WriteLine("SendSAE set buffer success");
                //}
                //else
                //{
                //    Console.WriteLine("Set buffer error");
                //}


                SocketAsyncEventArgs RecieveSAE = new SocketAsyncEventArgs();

                byte[] buffer = new byte[2048];
                RecieveSAE.SetBuffer(buffer,0,buffer.Length);
                RecieveSAE.Completed += new EventHandler<SocketAsyncEventArgs>(RecieveSAE_Completed);
                // 这里报错，RecieveSAE 对象没有绑定好？
                clientSocket.ReceiveAsync(RecieveSAE);
                Console.WriteLine("RecieveSAE set buffer success");
                //if (bufferManager.SetBuffer(RecieveSAE))
                //{

                //}
                //else
                //{
                //    Console.WriteLine("Set buffer error");
                //}


                Thread.Sleep(50);
            }
            TeleNum = 0;
        }

        private void RecieveSAE_Completed(object sender, SocketAsyncEventArgs e)
        {
            string msg;
            Socket sk = sender as Socket;
            byte[] data = e.Buffer;
            msg = System.Text.Encoding.UTF8.GetString(data);
            //*************************************************************************
            string datavalue = msg.Substring(39, 8).Replace("\0", "").Replace("R", "").Trim();  //此段报文解析，读取的变量不同解析方式应该也不同，待测试。

            Console.WriteLine(datavalue);
            //*************************************************************************
            //try
            //{
            //    EmptyNodeManager.CommunicationTagsHSX.hsxStaticInts[TeleNum] = int.Parse(datavalue);
            //}
            //catch
            //{
            //    Console.WriteLine("读取下位机数值异常");
            //}

            //bufferManager.FreeBuffer(e);

        }

        private void SendSAE_Completed(object sender, SocketAsyncEventArgs e)
        {
            //throw new NotImplementedException();
        }
        public void Communication()
        {
            while (CommCycle)
            {
                #region Test TCP ping
                try
                {
                    Ping pingSender = new Ping();
                    PingReply reply = pingSender.Send(ip);//第一个参数为ip地址，第二个参数为ping的时间
                    if (reply.Status == IPStatus.Success && connectFlag == false)
                    {
                        // first connected
                        Connect();
                        connectFlag = true;
                    }
                    if(reply.Status != IPStatus.Success) 
                    {
                        // ping error
                        connectFlag = false;
                    }
                    if(connectFlag && !SocketConnectBad())
                    {
                        Run();
                    }
                }
                catch(Exception ex) { }
                {
                }
                #endregion
                Thread.Sleep(200);
            }

        }

        private void Run()
        {
            if (!WriteReadShift)
            {
                WritePart();
            }
            if (WriteReadShift)
            {
                ReadPart();
            }
        }

        private void WritePart()
        {
            WriteReadShift = true;
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
