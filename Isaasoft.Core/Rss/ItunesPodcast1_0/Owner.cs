using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Isaasoft.Core.Rss.ItunesPodcast1_0
{
    public class Owner
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
    }
}
