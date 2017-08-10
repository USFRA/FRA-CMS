using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadCms.Web.Areas.NavigationBar.Models
{
    public class MenuItem
    {
        public enum ItemType
        {
            Page,
            Section
        }
        public int Id { get; set; }
        public int OverviewId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public int MenuOrder { get; set; }
        public ItemType Type { get; set; }
        public virtual IList<MenuItem> SubItems { get; set; }
    }
}