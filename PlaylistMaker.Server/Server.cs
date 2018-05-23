using System.Net;
using System.Text;
using System.Threading;
using PlaylistMaker.Logic;
using PlaylistMaker.Logic.Request;

namespace PlaylistMaker.Server
{
    internal class Server
    {
        private static void Main(string[] args)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:666/");
            var input = new Input();
            var console = new Thread(input.ConsoleRead);
            console.Start();

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

            } while (input.Value != "stop");

            listener.Stop();
        }
    }
}
