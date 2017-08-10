using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{
    public class VerPageHtml : CmsPageHtmlBase, IPageHtml, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("CmsPageHtmlId")]
        public int Id { get; set; }
    }
}