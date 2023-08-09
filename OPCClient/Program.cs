using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPCClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OpcClient opcclient = new OpcClient();

            opcclient.BeginRead();
            opcclient.writeitems(new int[] {1,2},new object[] {1,1,"over"});
            //Thread.Sleep(1000);
            opcclient.writeitem(0,0);
            //SocketClient socketclient = new SocketClient();
            Console.ReadKey();
        }


    }
}
