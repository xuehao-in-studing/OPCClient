using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace OPCClient
{
    internal class Communication
    {
        SocketClient socketClient = new SocketClient();
        OpcClient opcClient = new OpcClient();

        Thread clientThread;
        IPEndPoint ipep;
        //下位机IP地址，这里是托利多和扫码机的IP地址
        private static string ip;
        //下位机端口号
        private static string port;

        private static IPAddress address;

        private static string Sendtext;
        //连接状态标志位（是否可ping通）
        private static bool flags = false;

        private static string msg;

        private int TeleNum = 0;

        private static bool EnableCycle = true;

        private static bool WriteReadShift = false;
        public static string Group_Int_Writable;
        public static string Group_Int_Telegram;
        public static string Group_Int_TagName;

        public static string[] Group_Int_Writable_Arr;
        public static string[] Group_Int_Telegram_Arr;
        public static string[] Group_Int_TagName_Arr;

        public void LoadComAppConfiguration()
        {
            string OPC_IP = "", OPC_PORT = "", OPC_URL = "", OPC_Name="";
            XmlDocument doc = new XmlDocument();
            // 加载组态配置文件
            doc.Load("Config.xml");
            // 获取根节点
            XmlElement CommunicationConfiguration = doc.DocumentElement;

            XmlNodeList xmlNodeList = CommunicationConfiguration.ChildNodes;

            //XmlDocument SRV = new XmlDocument();
            //SRV.Load("HSX_Server.Config.xml");
            //XmlElement RootOPCUA = SRV.DocumentElement;
            //XmlNodeList xmlNodeList_OPC = RootOPCUA.ChildNodes;

            foreach (XmlNode item in xmlNodeList)
            {

                if (item.Name == "LowerMachine_IP_Address")
                {
                    socketClient.IP = item.InnerText;
                }
                if (item.Name == "LowerMachine_Port_Number")
                {
                    socketClient.Port = int.Parse(item.InnerText);
                }
                if (item.Name == "OPC_UA_Server_Name")
                {
                    opcClient.ServerName = item.InnerText;
                }
                if (item.Name == "OPC_UA_Server_IP_Address")
                {
                    opcClient.ServerIP = item.InnerText;
                }
                if (item.Name == "OPC_UA_Server_Port_Number")
                {
                    OPC_PORT = item.InnerText;
                }


                if (item.Name == "Group_Int")
                {
                    foreach (XmlNode GP_int_Item in item)
                    {
                        switch (GP_int_Item.Name)
                        {
                            case "Group_Int_Writable":
                                Group_Int_Writable = GP_int_Item.InnerText;
                                break;
                            case "Group_Int_Telegram":
                                Group_Int_Telegram = GP_int_Item.InnerText;
                                break;
                            case "Group_Int_TagName":
                                Group_Int_TagName = GP_int_Item.InnerText;
                                break;
                        }
                    }
                    Group_Int_Writable_Arr = Group_Int_Writable.Split(';');
                    Group_Int_Telegram_Arr = Group_Int_Telegram.Split(';');
                    Group_Int_TagName_Arr = Group_Int_TagName.Split(';');                              //写值下位机变量名
                    opcClient.TagNames = (string[])Group_Int_TagName_Arr.Clone();
                }

            }
        }



        public void Init()
        {
            //socketClient.Init();
            opcClient.Init();
        }
    }
}
