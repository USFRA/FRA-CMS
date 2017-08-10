using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{
    public class Carousel: IEntity
    {
        [Key]
        public int Id { get; set; }

        public int SlideId { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public bool Visible { get; set; }

        [Column("SlideContent", TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "SlideContent")]
        public string SlideContent { get; set; }
    }
}