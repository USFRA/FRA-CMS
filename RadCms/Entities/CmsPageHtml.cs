using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RadCms.Data;

namespace RadCms.Entities
{
    [Table("PageHtmls")]
    public class CmsPageHtml : CmsPageHtmlBase, IPageHtml, IEntity
    {
        [Key]
        [Column("CmsPageHtmlId")]
        public int Id { get; set; }
    }
}