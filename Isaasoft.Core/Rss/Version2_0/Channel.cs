using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Isaasoft.Core.Rss.Version2_0
{
    public class Channel
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "link", Namespace = Namespaces.Atom)]
        public string Link { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "docs")]
        public string Docs { get; set; }

        [XmlElement(ElementName = "lastBuildDate")]
        public string LastBuildDateString
        {
            get { return this.LastBuildDate.ToUniversalTime().ToString(); }
            set { this.LastBuildDate = DateTime.Parse(value); }
        }

        [XmlIgnore]
        public DateTime LastBuildDate { get; set; }

        [XmlElement(ElementName = "language")]
        public string Language { get; set; }

        [XmlElement(ElementName = "updatePeriod", Namespace = Namespaces.PurlRssSyndication1_0)]
        public string SyndUpdatePeriod { get; set; }

        [XmlElement(ElementName = "updateFrequency", Namespace = Namespaces.PurlRssSyndication1_0)]
        public int SyndUpdateFrequency { get; set; }

        [XmlElement(ElementName = "generator")]
        public string Generator { get; set; }

        [XmlElement(ElementName = "copyright")]
        public string Copyright { get; set; }

        [XmlElement(ElementName = "managingEditor")]
        public string ManagingEditor { get; set; }

        [XmlElement(ElementName = "webMaster")]
        public string WebMaster { get; set; }

        [XmlElement(ElementName = "ttl")]
        public string TTL { get; set; }

        [XmlElement(ElementName = "image")]
        public Image Image { get; set; }

        [XmlElement(ElementName = "subtitle", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItSubtitle { get; set; }

        [XmlElement(ElementName = "summary", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItSummary { get; set; }

        [XmlElement(ElementName = "keywords", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItKeywords { get; set; }

        [XmlElement(ElementName = "category", Namespace = Namespaces.ItunesPodcast1_0)]
        public ItunesPodcast1_0.Category ItCategory { get; set; }

        [XmlElement(ElementName = "author", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItAuthor { get; set; }

        [XmlElement(ElementName = "owner", Namespace = Namespaces.ItunesPodcast1_0)]
        public ItunesPodcast1_0.Owner ItOwner { get; set; }

        [XmlElement(ElementName = "block", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItBlock { get; set; }

        [XmlElement(ElementName = "explicit", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItExplicit { get; set; }

        [XmlElement(ElementName = "image", Namespace = Namespaces.ItunesPodcast1_0)]
        public ItunesPodcast1_0.Image ItImage { get; set; }

        [XmlElement(ElementName = "item")]
        public Item[] Items { get; set; }
    }
}
