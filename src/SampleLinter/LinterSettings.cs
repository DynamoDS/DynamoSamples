using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SampleLinter
{
    public class LinterSettings
    {
        [XmlElement("AllowedUngroupedNodes")]
        public int AllowedUngroupedNodes { get; set; } = 5;
        
        public static void SerializeModels(string filename, LinterSettings settings)
        {
            var xmls = new XmlSerializer(settings.GetType());
            var writer = new StreamWriter(filename);
            xmls.Serialize(writer, settings);
            writer.Close();
        }
        public static LinterSettings DeserializeModels(string filename)
        {
            var fs = new FileStream(filename, FileMode.Open);
            var xmls = new XmlSerializer(typeof(LinterSettings));
            return (LinterSettings)xmls.Deserialize(fs);
        }
    }
}
