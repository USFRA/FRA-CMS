using System.Collections.Generic;
using System.Web.Script.Serialization;
using RadCms.Entities;

namespace RadCms.Models
{
    public class JsTreeModel
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

        public string text { get; set; }
        public string linkUrl { get; set; }
        public bool hasChildren { get; set; }
        public bool isLazy { get; set; }
        public bool expanded { get; set; }
        public string icon { get; set; }
        public string id { get { return KeyId == null ? 
            (Type == "N" ? Type + Id : CmsPageBase.ToFriendlyId(Id)) : Type + KeyId; } }
        public List<JsTreeModel> children { get; set; }
    }
}