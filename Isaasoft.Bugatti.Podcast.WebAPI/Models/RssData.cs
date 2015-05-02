using Isaasoft.Core.Rss.Version2_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Isaasoft.Bugatti.Podcast.WebAPI.Models
{
    public class RssData
    {
        public int FromIndex { get; set; }

        public int Amount { get; set; }

        public string Url { get; set; }

        public string NextUrl { get; set; }

        public string PrevUrl { get; set; }

        public RssItem[] Data { get; set; }
    }
}