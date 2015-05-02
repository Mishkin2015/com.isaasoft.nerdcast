using Isaasoft.Core.Rss.Version2_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Isaasoft.Bugatti.Podcast.WebAPI.Models
{
    public class RssItemHash
    {
        public string HashKey { get; set; }

        public Item TheItem { get; set; }

        public string TitleComplexMetaphone { get; set; }

        public string SummaryComplexMetaphone { get; set; }
    }
}
