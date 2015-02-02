using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace OccTest
{
    /// <summary>
    /// XML Loader
    /// </summary>
    class XMLLoader<T>
    {
        public static Type type;
 
        public XMLLoader()
        {
            type = typeof(T);
        }
 
        public T LoadData(string filename)
        {
            T result;
            XmlSerializer xmlserializer = new XmlSerializer(type);
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            result = (T)xmlserializer.Deserialize(fs);
            fs.Close();
            return result;
        }
    }
}
