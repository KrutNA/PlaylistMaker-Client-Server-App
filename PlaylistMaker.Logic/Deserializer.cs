/*
 using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace PlaylistMaker.Logic
{
    internal static class Deserializer
    {
        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }
    }
}
*/