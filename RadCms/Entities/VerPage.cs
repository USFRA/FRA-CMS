using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities{
    public class VerPage : CmsPageBase, IPage, IEntity
    {
        [Key]
        public int VerId { get; set; }

        [MaxLength(2000)]
        [Index(IsUnique = false)]
        public string Url { get; set; }

        //Page Id
        public int Id { get; set; }

        [NotMapped]
        public string FriendlyId { get { return ToFriendlyId(this.Id); } }

        public virtual VerPageHtml Html { get; set; }

        //don't save navi node to remove constraint
        [Display(Name = "Navigation ")]
        [NotMapped]
        public NaviNode NaviNode { get; set; }

        [Column("NaviNode_Id")]
        public int NaviNodeId { get; set; }

        [NotMapped]
        public IPageHtml ContentHtml { get { return Html; } }    

        [MaxLength(100)]
        public string PublishedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime Published { get; set; }
    }
}