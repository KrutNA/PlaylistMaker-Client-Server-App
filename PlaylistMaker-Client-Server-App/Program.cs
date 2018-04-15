using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {

            }
            else
            {
                Console.WriteLine(
                    "Program need only 2 arguments:\n\t1) IP addres of Server\n\t2) Port for connection\nPress any key to exit ");
                Console.ReadKey();
            }
        }
    }
}
