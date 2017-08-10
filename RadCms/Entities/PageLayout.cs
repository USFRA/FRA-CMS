using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{
    public class PageLayout: IEntity
    {
        [Key]
        public int Id { get; set; }

        public int Order { get; set; }

        public bool IsVisible { get; set; }

        public virtual ContentType Type { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        public string Style { get; set; }

        [Column(TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        public string Template { get; set; }

        [Column(TypeName = "image")]
        public virtual byte[] Image { get; set; }
    }
}
