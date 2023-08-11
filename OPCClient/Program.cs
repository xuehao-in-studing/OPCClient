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

            Thread.Sleep(1000);
            opcclient.BeginRead();
            opcclient.writeitems(null, new object[] { 1, 0, "w69s" });

            if (opcclient.ResetEvent.WaitOne())
            {
                opcclient.Close();
                Console.ReadKey();
            }

        }


    }
}
