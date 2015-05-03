using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Isaasoft.Core.Rss.Version2_0
{
    public class Source
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
    }
}
