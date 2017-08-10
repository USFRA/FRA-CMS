using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadCms.Entities
{
    public class CmsPageHtmlBase
    {
        [Column("Summary", TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Column("Content", TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Column("Sidebar", TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Sidebar")]
        public string Sidebar { get; set; }

        [Column("Header", TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Header")]
        public string Header { get; set; }
    }
}