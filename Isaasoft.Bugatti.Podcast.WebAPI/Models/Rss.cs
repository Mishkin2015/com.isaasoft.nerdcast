using Isaasoft.Core.Rss.Version2_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Isaasoft.Bugatti.Podcast.WebAPI.Models
{
    public class Rss
    {
        public int RssId { get; set; }

        public string Name { get; set; }

        public string RssUri { get; set; }
    } 
}