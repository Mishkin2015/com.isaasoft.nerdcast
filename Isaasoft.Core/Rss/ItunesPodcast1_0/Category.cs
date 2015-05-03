using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Isaasoft.Core.Rss.ItunesPodcast1_0
{
    public class Category
    {
        [XmlAttribute(AttributeName = "text")]
        public string Value { get; set; }
    }
}
