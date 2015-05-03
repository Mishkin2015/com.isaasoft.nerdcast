using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Isaasoft.Core.Rss.Version2_0
{
    public class RssGuid
    {
        [XmlAttribute(AttributeName = "isPermaLink")]
        public string IsPermaLink { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
