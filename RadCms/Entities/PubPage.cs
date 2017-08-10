using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities{
    public class PubPage: CmsPageBase, IEntity, IPage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [MaxLength(2000)]
        [Index(IsUnique = true)]
        public string Url { get; set; }

        [NotMapped]
        public string FriendlyId { get { return ToFriendlyId(this.Id); } }

        public virtual PubPageHtml Html { get; set; }

        [Display(Name = "Navigation ")]
        public virtual NaviNode NaviNode { get; set; }

        [NotMapped]
        public IPageHtml ContentHtml { get { return Html; } }
    }
}