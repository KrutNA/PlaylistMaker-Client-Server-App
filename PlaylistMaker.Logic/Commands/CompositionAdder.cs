using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using PlaylistMaker.Logic.Model;
using PlaylistMaker.Logic.Stream;

namespace PlaylistMaker.Logic.Commands
{
    internal class CompositionAdder : ICommand
    {
        private const string Name = "add";
        public bool Displayability = true;
        private const int ArgsCount = 4;

        [XmlArray(ElementName = "arguments")]
        [XmlArrayItem("argument")]
        private Argument[] _args;

        public void Execute(string request, out string result)
        {
            var seralizer = new XmlSerializer(typeof(Argument[]));
            var tempPath = Environment.CurrentDirectory + "TempFile";
            File.WriteAllText(tempPath, request);
            using (var reader = new StreamReader(tempPath))
                _args = (Argument[])seralizer.Deserialize(reader);
            var playlist = new Playlist(_args[0].Value.EndsWith(".pls") ? _args[0].Value : $"{_args[0].Value}.pls");
            playlist.Compositions.Add(new Composition(_args[1].Value, _args[2].Value, _args[3].Value));
            playlist.Save();
            var argument = new Argument("Result", "Done\n");
            result = argument.Serialize();
        }

        public void Display(string result)
        {
            var argument = new Argument(result);
            var output = new Output();
            output.Execute($"{argument.Name}: {argument.Value}\n");
        }

        public string ReadArgs()
        {
            var input = new Input();
            var output = new Output();
            var tempPath = Environment.CurrentDirectory + "tempFile";
            var serializer = new XmlSerializer(typeof(Argument[]));
            _args = new []
            {
                new Argument("playlist", input.Execute("Playlist name: ")),
                new Argument("path", input.Execute("Path to composition: ")), 
                new Argument("author", input.Execute("Composition author: ")),
                new Argument("title", input.Execute("Composition title: ")), 
            };
            if (_args.Any(x => string.IsNullOrWhiteSpace(x.Value)))
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
