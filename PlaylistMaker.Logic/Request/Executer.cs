using System;
using System.Xml.Serialization;
using System.Linq;
using System.IO;
using PlaylistMaker.Logic.Commands;
using PlaylistMaker.Logic.Model;

namespace PlaylistMaker.Logic.Request
{
    public class Executer
    {
        private readonly string _request;
        private readonly string requestCommand;
        private RequestCommand _requestCommand;
        private readonly ICommand[] _commands;
        private string _result;
        private bool _isNotExecuted = true;

        public Executer(System.Net.HttpListenerRequest request)
        {
            _commands = new ICommand[]
            {
                new ClientCloser(),
                new CompositionAdder(),
                new CompositionRemover(),
                new CompositionSearchEngine(),
                new CompositionsListDisplayer(),
                //new CompositionsListDisplayerFull(),
                new PlaylistsListDisplayer(),
                new PMHelpDisplayer(),
            };
            var reader = new StreamReader(request.InputStream);
            this.requestCommand = reader.ReadLine() + "\n";
            this.requestCommand += reader.ReadLine() + "\n";
            this.requestCommand += reader.ReadLine() + "\n";
            this.requestCommand += reader.ReadLine();
            this._request = reader.ReadToEnd();
        }
        
        public string GetResult()
        {
            return _isNotExecuted ? null : _result;
        }

        /// <summary>
        /// Server always gets correct request and it's always executes
        /// </summary>
        public void Execute()
        {
            var tempPath = Environment.CurrentDirectory + "TempFile";
                File.WriteAllText(tempPath, requestCommand);
            var serializer = new XmlSerializer(typeof(RequestCommand));
            using (var streamReader = new StreamReader(tempPath))
            {
                _requestCommand = (RequestCommand)serializer.Deserialize(streamReader);
            }
            var command = _commands.FirstOrDefault(x => x.GetName() == _requestCommand.Name);
            command.Execute(_request, out _result);
            if (_result != null)
                this._isNotExecuted = false;
        }
    }
}
