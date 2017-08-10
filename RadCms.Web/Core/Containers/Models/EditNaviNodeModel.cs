using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Core.Containers.Models
{
    public class EditNaviNodeModel
    {
        [Key]
        public int Id { get; set; }

        public int MenuOrder { get; set; }

        [Display(Name = "Parent Node")]
        public int? ParentId { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        [Display(Name = "Section Name")]
        public string NodeName { get; set; }

        [Display(Name = "Default Page")]
        public int? DefaultPageId { get; set; }

        //[MaxLength(2048)]
        //[Display(Name = "Breadcrumb")]
        //public string Breadcrumb { get; set; }
    }
}