using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RadCms.Data;
using Newtonsoft.Json;

namespace RadCms.Entities
{
    public class NavGroup: IEntity
    {
        [Key]
        public int Id { get; set; }

        public int Index { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Link { get; set; }

        public virtual List<NavItem> Items { get; set; }
        public virtual List<NavGroup> SubGroups { get; set; }

        [JsonIgnore]
        public virtual NavGroup Parent { get; set; }
    }
}
