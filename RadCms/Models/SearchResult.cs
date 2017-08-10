using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadCms.Models
{
    public class SearchResult
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string MIMEType { get; set; }
        public string Subject { get; set; }
        public string Date { get; set; }
        public int NodeNumber { get; set; }
        public int LeafNumber { get; set; }
    }
}
