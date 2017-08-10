using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;
using System.Web.Mvc;

namespace RadCms.Web.Areas.Comment.Entities
{
    public class Comment: IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PageId { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [MaxLength(50, ErrorMessage = "The name is too long."), MinLength(1)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "The email address is too long."), MinLength(1)]
        [Required(ErrorMessage = "The email address is required.")]
        [EmailAddress(ErrorMessage = "The email address is  invalid.")]
        public string Email { get; set; }

        [Column("Content", TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        [Required]
        [AllowHtml]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Column("OrigContent", TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "OrigContent")]
        [AllowHtml]
        public string OrigContent { get; set; }

        [Display(Name = "Status")]
        public int? Status { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}