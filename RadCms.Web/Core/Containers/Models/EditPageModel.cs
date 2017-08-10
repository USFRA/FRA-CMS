using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Core.Containers.Models
{
    public class EditPageModel
    {
        [Key]
        public int Id { get; set; }

        public int MenuOrder { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        [Display(Name = "Page Title")]
        public string Title { get; set; }

        [MaxLength(127)]
        [Display(Name = "Navigation Title")]
        public string NaviTitle { get; set; }
        
        [MaxLength(511)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [MaxLength(255)]
        [Display(Name = "Keywords")]
        public string Keywords { get; set; }

        [MaxLength(255)]
        [Display(Name = "Friendly URL")]
        public string FriendlyUrl { get; set; }

        [Display(Name = "Layout ID")]
        public int Layout { get; set; }

        [Display(Name = "Navigation ")]
        public int? NaviNodeId { get; set; }

        [Display(Name = "Navigation Heading")]
        public int? NaviHeadingId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Sidebar")]
        public string Sidebar { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Header")]
        public string Header { get; set; }

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; }

        [Display(Name = "Hide This Column")]
        public bool IsHideRightColumn { get; set; }

        [Display(Name = "Hide Breadcrumb/Title")]
        public bool IsHideBreadcrumb { get; set; }

        public int Status { get; set; }

        [Display(Name = "Post Date")]
        public DateTime? Created { get; set; }

        public string Type { get; set; }

        [Display(Name = "Allow Comments from other people")]
        public bool IsCommentable { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Summary")]
        public string Summary { get; set; }
    }
}