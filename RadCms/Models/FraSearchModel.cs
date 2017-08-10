using System.Collections.Generic;

namespace RadCms.Models
{
    public class FraSearchModel
    {
        public string Time { get; set; }
        public string Query { get; set; }
        public string Sort { get; set; }
        public int Start { get; set; }
        public string PreviousLink { get; set; }
        public string NextLink { get; set; }
        public int Total { get; set; }
        public string Site { get; set; }
        public string Suggestion { get; set; }
        public ICollection<SearchResult> ResultList { get; set; }
    }
}
