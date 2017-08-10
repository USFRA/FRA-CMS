using RadCms.Data;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Entities
{
    public class Organization: IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Organization")]
        public string Title { get; set; }

        [Display(Name = "Email Domain(like dot.gov)")]
        [MaxLength(256)]
        public string EmailDomain { get; set; }

        [Display(Name = "Allow Non-official Emails")]
        public bool AllowOtherEmails { get; set; }

        [Display(Name = "Registration Needs Approval")]
        public bool ApprovalNeeded { get; set; }

        [Display(Name = "Default Roles")]
        [MaxLength(512)]
        public string DefaultRoles { get; set; }
    }
}
