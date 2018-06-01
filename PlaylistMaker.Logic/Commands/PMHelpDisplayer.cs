using System;
using System.IO;
using System.Xml.Serialization;
using PlaylistMaker.Logic.Stream;

namespace PlaylistMaker.Logic.Commands
{
    public class PMHelpDisplayer : ICommand
    {
        private const string Name = "help";
        public bool Displayability = true;
        private const int ArgsСount = 0;
        
        public struct Result
        {
            [XmlArray(ElementName = "commands")]
            [XmlArrayItem("command")]
            public string[] Commands { get; set; }
        }

        public void Execute(string request, out string result)
        {
            var newResult = new Result
            {
                Commands = new[]
                {
                    "help - to display this message",
                    "ls - to display all playlists names",
                    "list - to display all compositions in the current playlist",
                    //"listf - to display all info about compositions in the current playlist",
                    "add - to add composition to the current playlist",
                    "search - to search composition in the current playlist",
                    "rm - to remove composition from the current playlist",
                    "exit - to close this program"
                }
            };
            var tempPath = Environment.CurrentDirectory + "TempFile";
            var serializer = new XmlSerializer(typeof(Result));
            using (var writer = new StreamWriter(tempPath))
            {
                serializer.Serialize(writer, newResult);
            }
            result = File.ReadAllText(tempPath);
            File.Delete(tempPath);
        }

        public void Display(string result)
        {
            var newResult = new Result();
            var output = new Output();
            var serializer = new XmlSerializer(newResult.GetType());
            var tempPath = Environment.CurrentDirectory + "TempFile";
            File.WriteAllText(tempPath, result);
            using (var reader = new StreamReader(tempPath))
            {
                newResult = (Result) serializer.Deserialize(reader);
            }
            File.Delete(tempPath);
            foreach (var command in newResult.Commands)
            {
                output.Execute($"\t{command}\n");
            }
        }

        public string ReadArgs()
        {
            return null;
        }
        
        public string GetName()
        {
            return Name;
        }

        public int GetArgsCount()
        {
            return ArgsСount;
        }

        public bool IsDisplayable()
        {
            return Displayability;
        }
    }
}
