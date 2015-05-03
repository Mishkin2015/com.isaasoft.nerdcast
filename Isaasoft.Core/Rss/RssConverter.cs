using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Isaasoft.Core.Rss
{
    public static class RssConverter
    {
        public static T ConvertTo<T>(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(stream);
        }

        public static T ConvertTo<T>(string url)
        {
            var webRequest = HttpWebRequest.CreateHttp(url);
            webRequest.Method = WebRequestMethods.Http.Get;

            var result = default(T);

            using (var response = webRequest.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                result = RssConverter.ConvertTo<T>(stream);

                stream.Close();
                response.Close();
            }

            return result;
        }
    }
}
