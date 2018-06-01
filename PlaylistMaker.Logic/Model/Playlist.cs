using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace PlaylistMaker.Logic.Model
{
    public class Playlist
    {
        [XmlElement(ElementName = "playlist")]
        [XmlArrayItem("composition")]
        public List<Composition> Compositions { get; set; }

        [XmlIgnore]
        private readonly string _path;
        
        public Playlist(string path)
        {
            Compositions = new List<Composition>();
            _path = path;
            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    reader.ReadLine();
                    var compositionsCount = Convert.ToInt16(reader.ReadLine().Split('=')[1]);
                    for (var i = 0; i < compositionsCount; i++)
                    {
                        reader.ReadLine();
                        var compositionPath = reader.ReadLine();
                        var fullTitle = reader.ReadLine();
                        compositionPath = compositionPath.Split('=')[1];
                        fullTitle = fullTitle.Split('=')[1];
                        Compositions.Add(new Composition(compositionPath, fullTitle));
                    }
                }
            }
        }

        public void Save()
        {
            using (var writer = new StreamWriter(_path))
            {
                writer.Write($"[Playlist]\nNumberOfEntries={this.Compositions.Count}\n");
                for (var i = 1; i <= Compositions.Count; i++)
                {
                    writer.Write($"\nFile{i}={Compositions[i-1].Path}\nTitle{i}={Compositions[i-1].FullTitle}\n");
                }
            }
        }
    }
}
