using System;
using System.Net;
using System.Text;
using System.Threading;
using PlaylistMaker.Logic.Request;
using PlaylistMaker.Logic.Stream;

namespace PlaylistMaker.Server
{
    internal class Server
    {
        private static HttpListener listener;

        private static void CheckInput()
        {
            var input = new Input();
            while (input.Execute().ToLower() != "stop") { }
            listener.Stop();
            Environment.Exit(0);
        }

        private static void Main(string[] args)
        {
            Console.Title = "Server";
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:10666/");
            Console.WriteLine("input \"stop\" to close app");
            var check = new Thread(CheckInput);
            check.Start();
            listener.Start();
            do
            {
                var context = listener.GetContext();
                var request = context.Request;
                var executer = new Executer(request);
                executer.Execute();
                var buffer = Encoding.UTF8.GetBytes(executer.GetResult());
                var response = context.Response;
                response.ContentLength64 = buffer.Length;
                using (var output = response.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }

            } while (true);
        }
    }
}
