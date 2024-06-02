using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!DBManager.Connect("mysql", "8.130.122.189", 3306, "root", ""))
            {
                return;
            }
            NetManager.StartLoop(8888);
        }
    }
}
