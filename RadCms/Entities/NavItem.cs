using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using RadCms.Data;
namespace RadCms.Entities
{
    public class NavItem: IEntity
    {
        [Key]
        public int Id { get; set; }

        public int Index { get; set; }

        [Required]
        [StringLength(100)]
        public string Text { get; set; }

        [Required]
        [StringLength(2000)]
        public string Link { get; set; }

        [JsonIgnore]
        public virtual NavGroup Group { get; set; }
    }
}
