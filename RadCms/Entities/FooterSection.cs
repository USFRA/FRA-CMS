using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RadCms.Data;

namespace RadCms.Entities
{
    public class FooterSection: IEntity
    {
        public enum SectionType:int{
            Vertical = 0,
            Horizontal = 1
        }
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [Required, Range(1, 4)]
        public int Column { get; set; }

        [Required]
        public int Order { get; set; }
        
        public SectionType Type { get; set; }

        public virtual ICollection<FooterItem> Items { get; set; }
    }
}
