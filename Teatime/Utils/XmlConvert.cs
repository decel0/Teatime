using System;
using System.IO;
using System.Xml.Serialization;

namespace Teatime.Utils
{
    public class XmlConvert
    {
        public static string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize, ns);
                return textWriter.ToString();
            }
        }

        public static T DeserializeObject<T>(string objectData)
        {
            return (T) XmlDeserializeFromString(objectData, typeof(T));
        }

        private static object XmlDeserializeFromString(string objectData, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}