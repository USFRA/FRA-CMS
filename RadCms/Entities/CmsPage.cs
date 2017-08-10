using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{
    [Table("Pages")]
    public class CmsPage: CmsPageBase, IEntity, IPage
    {
        [Key]
        [Display(Name = "Page Id")]
        public int Id { get; set; }

        [MaxLength(2000)]
        [Index(IsUnique = true)]
        public string Url { get; set; }

        [NotMapped]
        public string FriendlyId { get { return ToFriendlyId(this.Id); } }

        public virtual CmsPageHtml Html { get; set; }

        [Display(Name = "Navigation")]
        public virtual NaviNode NaviNode { get; set; }

        [NotMapped]
        public IPageHtml ContentHtml { get { return Html; } }
    }
}