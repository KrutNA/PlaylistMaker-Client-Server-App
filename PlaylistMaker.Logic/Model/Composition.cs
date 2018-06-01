using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace PlaylistMaker.Logic.Model
{
    /// <summary>
    /// Composition object
    /// </summary>
    public class Composition
    {
        // Path to composition file
        [XmlElement(ElementName = "path")]
        public string Path { get; set; }

        // Author's name
        [XmlElement(ElementName = "author")]
        public string Author { get; set; }

        // Composition's title
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        // Full music name like
        //"%Author%   -   %Title%"
        [XmlIgnore]
        public string FullTitle { get; set; }

        //Length of composition in seconds, sets always as "-1" (unknown)
        [XmlIgnore]
        public string Length { get; set; } = "-1";

        // Create object then writing
        public Composition(
            string path, 
            string author, 
            string title)
        {
            this.FullTitle = author + "  -  " + title;
            this.Author = author;
            this.Path = path;
            this.Title = title;
        }

        // Create object then reading
        public Composition(
            string path, 
            string fullTitle)
        {
            this.FullTitle = fullTitle;
            this.Author = Regex.Split(fullTitle, "  -  ")[0];
            this.Title = Regex.Split(fullTitle, "  -  ")[1];
            this.Path = path;
        }

        public Composition() { }
    }
}