using System;
using System.Web.Mvc;

namespace RadCms.Core.Containers.Models
{
    using Entities;

    public class ActionBarModel
    {
        public UrlHelper Url { get; set; }
        public bool IsPublished { get; set; }
        public int Status { get; set; }
        public int SectionId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime LastModified { get; set; }
        public PermissionType AccessMode { get; set; }
        public string FriendlyId { get; set; }
        public int Id { get; set; }
        public string ReturnUrl { get; set; }
        public ContentType Type { get; set; }
        public ContentType SectionType { get; set; }
    }
}