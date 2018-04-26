using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DM.Infrastructure.Helper
{
    public static class Xml
    {
        //序列化对象
        public static string Serialize<T>(T model)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    // 强制指定命名空间，覆盖默认的命名空间  
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);
                    new XmlSerializer(typeof(T)).Serialize(writer, model, namespaces);
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        //反序列化xml
        public static T Deserialize<T>(string strXml)
        {
            using (StringReader sr = new StringReader(strXml))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(sr);
            }
        }
    }
}
