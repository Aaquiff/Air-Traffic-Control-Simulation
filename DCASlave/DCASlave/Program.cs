using DCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DCASlave
{
    class Program
    {
        static Slave[] slaves = new Slave[4];
        static void Main(string[] args)
        {

            for (int i = 0; i < 4; i++)
            {
                slaves[i] = new Slave();
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
