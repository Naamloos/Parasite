using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Host
{
    class Program
    {
        static Beacon bc = new Beacon(9999);
        static Server sv = new Server(6969);
        static bool running = true;

        static void Main(string[] args)
        {
            bc.Start();
            sv.Start();
            while(running)
            {

            }
        }
    }
}
