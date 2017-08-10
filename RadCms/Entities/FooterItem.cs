using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{

    public class FooterItem: IEntity
    {
        [Key]
        public int Id { set; get; }

        public virtual FooterSection Section { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Link { get; set; }

        [MaxLength(6)]
        public string Target { get; set; }

        public int Index { get; set; }

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime Created { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime Modified { get; set; }
    }
}
