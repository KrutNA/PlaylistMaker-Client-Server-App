using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using PlaylistMaker.Logic.Model;
using PlaylistMaker.Logic.Stream;

namespace PlaylistMaker.Logic.Commands
{
    internal class CompositionSearchEngine : ICommand
    {
        private const string Name = "search";
        public bool Displayability = true;
        private const int ArgsCount = 4;

        [XmlArray(ElementName = "arguments")]
        [XmlArrayItem("argument")]
        private Argument[] _args;

        public void Execute(string request, out string result)
        {
            var serializer = new XmlSerializer(typeof(Argument[]));
            var tempPath = Environment.CurrentDirectory + "TempFile";
            File.WriteAllText(tempPath, request);
            using (var reader = new StreamReader(tempPath))
                _args = (Argument[])serializer.Deserialize(reader);
            File.Delete(tempPath);
            if (!File.Exists(_args[0].Value) && !File.Exists($"{_args[0].Value}.pls"))
            {
                var requestCommand = new RequestCommand("result");
                result = requestCommand.Serialize();
                var argument = new Argument("Result", "Playlist not found");
                result += "\n" + argument.Serialize();
                return;
            }
            _args[1].Value = string.IsNullOrWhiteSpace(_args[1].Value) ? "" : _args[1].Value;
            _args[2].Value = string.IsNullOrWhiteSpace(_args[1].Value) ? "" : _args[2].Value;
            _args[3].Value = string.IsNullOrWhiteSpace(_args[1].Value) ? "" : _args[3].Value;
            var playlist = new Playlist(_args[0].Value.EndsWith(".pls") ? _args[0].Value : $"{_args[0].Value}.pls");
            List<Composition> compositions = playlist.Compositions.FindAll(x =>
                x.Path.Contains(_args[1].Value) &&
                x.Author.Contains(_args[2].Value) &&
                x.Title.Contains(_args[3].Value));
            if (compositions.Any())
            {
                var requestCommand = new RequestCommand("compositions");
                result = requestCommand.Serialize();
                serializer = new XmlSerializer(typeof(List<Composition>));
                using (var writer = new StreamWriter(tempPath))
                    serializer.Serialize(writer, compositions);
                result += "\n" + File.ReadAllText(tempPath);
                File.Delete(tempPath);
            }
            else
            {
                var requestCommand = new RequestCommand("result");
                result = requestCommand.Serialize();
                var argument = new Argument("Result", "Not found");
                result += "\n" + argument.Serialize();
            }
        }

        public void Display(string result)
        {
            var tempPath = Environment.CurrentDirectory + "TempFile";
            RequestCommand request;
            string searchResult;
            var output = new Output();
            var serializer = new XmlSerializer(typeof(RequestCommand));
            File.WriteAllText(tempPath, result);
            var requestCommand = "";
            using (var reader = new StreamReader(tempPath))
            {
                requestCommand += reader.ReadLine() + "\n";
                requestCommand += reader.ReadLine() + "\n";
                requestCommand += reader.ReadLine() + "\n";
                requestCommand += reader.ReadLine();
                searchResult = reader.ReadToEnd();
            }
            File.Delete(tempPath);
            File.WriteAllText(tempPath, requestCommand);
            using (var reader = new StreamReader(tempPath))
                request = (RequestCommand)serializer.Deserialize(reader);
            File.Delete(tempPath);
            if (request.Name == "result")
            {
                var argument = new Argument(searchResult);
                output.Execute($"\n{argument.Name}: {argument.Value}");
            }
            else
            {
                List<Composition> compositions;
                serializer = new XmlSerializer(typeof(List<Composition>));
                File.WriteAllText(tempPath, searchResult);
                using (var reader = new StreamReader(tempPath))
                    compositions = (List<Composition>)serializer.Deserialize(reader);
                output.Execute("Compositions:");
                var i = 0;
                foreach (var composition in compositions)
                {
                    i++;
                    output.Execute($"\n\t{i}) {composition.Author} - {composition.Title}");
                }
                File.Delete(tempPath);
            }
            output.Execute("\n");
        }

        public string ReadArgs()
        {
            var input = new Input();
            var output = new Output();
            var tempPath = Environment.CurrentDirectory + "TempFile";
            var serializer = new XmlSerializer(typeof(Argument[]));
            _args = new[]
            {
                new Argument("playlist", input.Execute("Playlist name: ")),
                new Argument("path", input.Execute("Path to composition: ")),
                new Argument("author", input.Execute("Composition author: ")),
                new Argument("title", input.Execute("Composition title: ")),
            };
            if (string.IsNullOrWhiteSpace(_args[0].Value) ||
                (string.IsNullOrWhiteSpace(_args[1].Value) &&
                 string.IsNullOrWhiteSpace(_args[2].Value) &&
                 string.IsNullOrWhiteSpace(_args[3].Value)))
            {
                output.Execute("One or more arguments is null or whitespace!\n");
                return null;
            }
            using (var writer = new StreamWriter(tempPath))
            {
                serializer.Serialize(writer, _args);
            }
            var result = File.ReadAllText(tempPath);
            File.Delete(tempPath);
            return result;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetArgsCount()
        {
            return ArgsCount;
        }

        public bool IsDisplayable()
        {
            return Displayability;
        }
    }
}
