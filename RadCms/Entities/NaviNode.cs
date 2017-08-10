using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RadCms.Data;

namespace RadCms.Entities
{
    /**
     * Section
     **/
    public class NaviNode: IEntity
    {
        [Key]
        public int Id { get; set; }
        public int MenuOrder { get; set; }

        [Display(Name = "Parent")]
        public virtual NaviNode Parent { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        [Display(Name = "Node Name")]
        public string NodeName { get; set; }

        [Display(Name = "Default Page")]
        public int? DefaultPageId { get; set; }

        [MaxLength(2048)]
        [Display(Name = "Breadcrumb")]
        public string Breadcrumb { get; set; }

        public virtual ICollection<NaviNode> SubNodes { get; set; }
        public virtual ICollection<NaviHeading> NaviHeadings { get; set; }
        public virtual ICollection<CmsPage> Pages { get; set; }

        public bool IsSecure { get; set; }

        public virtual ContentType Type { get; set; }

        public bool Hidden { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public System.DateTime Created { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }
        public System.DateTime Modified { get; set; }
    }
}