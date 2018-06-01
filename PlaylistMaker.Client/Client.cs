using System;
using System.Text;
using System.Net.Http;
using System.Threading;
using PlaylistMaker.Logic.Request;
using PlaylistMaker.Logic.Stream;

namespace PlaylistMaker.Client
{
    internal class Client
    {
        private static void Main(string[] args)
        {
            Console.Title = "Client";
            var output = new Output();
            var input = new Input();
            if (args.Length == 2)
            {
                while (true)
                {
                    var creator = new Creator();
                    if (!creator.Execute())
                        return;
                    var client = new HttpClient();
                    var content =
                        new ByteArrayContent(Encoding.UTF8.GetBytes(creator.GetResult()));
                    try
                    {
                    var response = client.PostAsync($"http://{args[0]}:{args[1]}/", content);
                    var message = response.Result.Content.ReadAsStringAsync();
                    creator.DisplayResponse(message.Result);
                    }
                    catch(Exception e)
                    {
                        output.Execute($"Can't connect to the server.\n{e}");
                        input.ReadKey();
                        return;
                    }
                }
            }
            else
            {
                output.Execute(
                    "Program need only 2 arguments:\n\t1) IP addres of Server\n\t2) Port for connection\nPress any key to exit");
                input.ReadKey();
            }
        }
    }
}
