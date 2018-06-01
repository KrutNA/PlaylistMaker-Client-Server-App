using System;
using System.IO;
using System.Xml.Serialization;

namespace PlaylistMaker.Logic.Model
{
    [Serializable]
    public class Argument
    {
        [XmlElement(ElementName="name")]
        public string Name { get; set; }
        [XmlElement(ElementName="value")]
        public string Value { get; set; }

        public Argument() { }

        public Argument(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public Argument(string request)
        {
            var serializer = new XmlSerializer(typeof(Argument));
            var tempPath = Environment.CurrentDirectory + "TempFile";
            File.WriteAllText(tempPath, request);
            using (var reader = new StreamReader(tempPath))
            {
                var argument = (Argument)serializer.Deserialize(reader);
                this.Name = argument.Name;
                this.Value = argument.Value;
            }
            File.Delete(tempPath);
        }

        public string Serialize()
        {
            var serializer = new XmlSerializer(this.GetType());
            var tempPath = Environment.CurrentDirectory + "TempFile";
            using (var writer = new StreamWriter(tempPath))
                serializer.Serialize(writer, this);
            var result = File.ReadAllText(tempPath);
            File.Delete(tempPath);
            return result;
        }
    }
}