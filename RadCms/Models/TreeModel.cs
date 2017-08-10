using System.Collections.Generic;
using System.Web.Script.Serialization;
using RadCms.Entities;

namespace RadCms.Models
{
    public class TreeModel
    {
        [ScriptIgnore]
        public int Id { get; set; }

        [ScriptIgnore]
        public string KeyId { get; set; }

        //[ScriptIgnore]
        public int MenuOrder { get; set; }
        
        [ScriptIgnore]
        public string Type { get; set; }

        [ScriptIgnore]
        public object Item { get; set; }

        public string title { get; set; }
        public string linkUrl { get; set; }
        public bool hasChildren { get; set; }
        public bool isLazy { get; set; }
        public bool expanded { get; set; }
        public string imageUrl { get; set; }
        public string id { get { return KeyId == null ? 
            (Type == "N" ? Type + Id : CmsPage.ToFriendlyId(Id)) : Type + KeyId; } }
        public List<TreeModel> items { get; set; }
    }
}