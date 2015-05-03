using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Isaasoft.Core.Rss.Version2_0
{
    public class Item
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }

        [XmlElement(ElementName = "author")]
        public string Author { get; set; }

        [XmlElement(ElementName = "comments")]
        public string Comments { get; set; }

        [XmlElement(ElementName = "pubDate")]
        public string PubDateString
        {
            get { return this.PubDate.ToUniversalTime().ToString(); }
            set { this.PubDate = DateTime.Parse(value); }
        }

        [XmlIgnore]
        public DateTime PubDate { get; set; }

        [XmlElement(ElementName = "creator", Namespace = Namespaces.PurlDCElements1_1)]
        public string DCCreator { get; set; }

        [XmlElement(ElementName = "category")]
        public string[] Category { get; set; }

        [XmlElement(ElementName = "guid")]
        public RssGuid Guid { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "source")]
        public Source Source { get; set; }

        [XmlElement(ElementName = "encoded", Namespace = Namespaces.PurlRssContent1_0)]
        public string ContentEncoded { get; set; }

        [XmlElement(ElementName = "commentRss", Namespace = Namespaces.WellFormedWeb)]
        public string WfwCommentRss { get; set; }

        [XmlElement(ElementName = "comments", Namespace = Namespaces.PurlRssSlash1_0)]
        public string SlComments { get; set; }

        [XmlElement(ElementName = "enclosure")]
        public Enclosure Enclosure { get; set; }

        [XmlElement(ElementName = "duration", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItDuration { get; set; }

        [XmlElement(ElementName = "subtitle", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItSubtitle { get; set; }

        [XmlElement(ElementName = "summary", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItSummary { get; set; }

        [XmlElement(ElementName = "keywords", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItKeywords { get; set; }

        [XmlElement(ElementName = "author", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItAuthor { get; set; }

        [XmlElement(ElementName = "explicit", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItExplicit { get; set; }

        [XmlElement(ElementName = "block", Namespace = Namespaces.ItunesPodcast1_0)]
        public string ItBlock { get; set; }
    }
}
