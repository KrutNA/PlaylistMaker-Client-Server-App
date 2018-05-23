using System;
using System.Net.Http;
using System.Text;
using PlaylistMaker.Logic.Request;

namespace PlaylistMaker.Client
{
    internal class Client
    {
        private static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                var creator = new Creator();
                if (!creator.Execute())
                    return;
                var client = new HttpClient();

                var content =
                    new ByteArrayContent(Encoding.UTF8.GetBytes(creator.GetResult()));
                try
                {
                    client.PostAsync($"http://{args[0]}:{args[1]}", content);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Can't connect to the server.\n{e}");
                }
            }
            else
            {
                Console.WriteLine(
                    "Program need only 2 arguments:\n\t1) IP addres of Server\n\t2) Port for connection\nPress any key to exit");
                Console.ReadKey();
            }
        }
    }
}
