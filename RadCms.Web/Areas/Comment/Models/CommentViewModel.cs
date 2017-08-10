using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Web.Areas.Comment.Models
{
    public class CommentViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email;
        public string Name;
        public List<Entities.Comment> Comments;
        public bool Status;
        public int PageId;
        public bool IsEditState;
        public string Url;
    }
}
