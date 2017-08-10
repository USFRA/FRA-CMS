using System;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Entities
{
    public class CmsPageBase
    {
        public const int STATUS_UNPUBLISHED = -1;
        public const int STATUS_NORMAL = 0;
        public const int STATUS_EDITING_START = 1;
        public const int STATUS_CHANGE_SAVED = 2;
        public const int STATUS_EDITING_AGAIN = 3;
        public const int STATUS_EDITING_BY_OTHERS = 4;
        public const int STATUS_ARCHIVED = 5;

        public static NaviNode FindBaseNode(NaviNode node)
        {
            var parentNode = node;
            while (parentNode != null && parentNode.Parent != null && parentNode.Parent.Parent != null)
            {
                parentNode = parentNode.Parent;
            }
            return parentNode;
        }

        public static NaviNode FindBaseNodeForMenu(NaviNode node)
        {
            var parentNode = node;
            while (parentNode != null 
                && parentNode.Parent != null 
                && parentNode.Parent.Parent != null)
            {
                parentNode = parentNode.Parent;
            }
            return parentNode;
        }


        public static int FindExpandableNode(NaviNode node)
        {
            var sectionId = -1;
            var section = node;
            while (section != null
                && section.Parent != null
                && section.Parent.Parent != null)
            {
                sectionId = section.Id;
                section = section.Parent;
            }
            return sectionId;
        }

        public static NaviNode FindBaseNode(IPage page)
        {
            var parentNode = page.NaviNode;
            return FindBaseNode(parentNode);
        }
        

        public static string ToFriendlyId(int id)
        {
            return "P" + Convert.ToString(id + 10000).Substring(1);
        }

        public static bool IsFriendlyId(string id)
        {
            return id != null && id.StartsWith("P") && id.Length == 5;
        }

        public static int FromFriendlyId(string id)
        {
            int innerId = 0;

            if (id == null)
            {
                return 1;
            }
            else if (id.StartsWith("P"))
            {
                Int32.TryParse(id.Substring(1), out innerId);
            }
            else
            {
                Int32.TryParse(id, out innerId);
            }

            return innerId;
        }

        //attributes

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

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; }

        public int Layout { get; set; }

        public int Status { get; set; }

        public virtual ContentType Type { get; set; }

        public bool Commentable { get; set; }

        public bool Hidden { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Created { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Modified { get; set; }
    }
}