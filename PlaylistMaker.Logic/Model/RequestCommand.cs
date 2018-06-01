using System;
using System.IO;
using System.Xml.Serialization;

namespace PlaylistMaker.Logic.Model
{
    public class RequestCommand
    {
        [XmlElement(ElementName = "commandName")]
        public string Name { get; set; }

        public RequestCommand() { } 

        public RequestCommand(string name)
        {
            this.Name = name;
        }

        public string Serialize()
        {
            var serializer = new XmlSerializer(this.GetType());
            var tempPath = Environment.CurrentDirectory + "TempFile";
            if (!File.Exists(tempPath))
                tempPath += this.GetHashCode();
            using (var writer = new StreamWriter(tempPath))
                serializer.Serialize(writer, this);
            var result = File.ReadAllText(tempPath);
            File.Delete(tempPath);
            return result;
        }
    }
}
