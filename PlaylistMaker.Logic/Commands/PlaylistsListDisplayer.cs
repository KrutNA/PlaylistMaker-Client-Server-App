using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using PlaylistMaker.Logic.Stream;

namespace PlaylistMaker.Logic.Commands
{
    internal class PlaylistsListDisplayer : ICommand
    {
        private const string Name = "ls";
        public bool Displayability = true;
        private const int ArgsCount = 0;
        
        [XmlArray(ElementName = "files")]
        [XmlArrayItem("file")]
        private List<string> _fileNames;

        public void Execute(string request, out string result)
        {
            _fileNames = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.pls").ToList();
            _fileNames = _fileNames.Select(x => Path.GetFileName(x)).ToList();
            var tempFile = Environment.CurrentDirectory + "TempFile";
            var serializer = new XmlSerializer(_fileNames.GetType());
            using (var writer = new StreamWriter(tempFile))
            {
                serializer.Serialize(writer, _fileNames);
            }
            result = File.ReadAllText(tempFile);
            File.Delete(tempFile);
        }

        public void Display(string result)
        {
            var output = new Output();
            _fileNames = new List<string>();
            var serializer = new XmlSerializer(typeof(List<string>));
            var tempPath = Environment.CurrentDirectory + "TempFile";
            File.WriteAllText(tempPath, result);
            using (var reader = new StreamReader(tempPath))
            {
                _fileNames = (List<string>)serializer.Deserialize(reader);
            }
            File.Delete(tempPath);
            output.Execute("Playlists:\n");
            
            _fileNames.ForEach(x => output.Execute($"\t{x}\n"));
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
            return ArgsCount;
        }

        public bool IsDisplayable()
        {
            return Displayability;
        }
    }
}
