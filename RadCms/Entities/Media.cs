using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{
    public class Media: IEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(255), MinLength(1)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public virtual MediaFile File { get; set; }
        [Column("NaviNode_Id")]
        public int? NaviNodeId { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime Created { get; set; }

        [MaxLength(100),]
        public string ModifiedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime Modified { get; set; }    
    }
}