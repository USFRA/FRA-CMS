using System;
namespace RadCms.Entities
{
    public interface IPage
    {
        DateTime Created { get; set; }
        string CreatedBy { get; set; }
        string Description { get; set; }
        string FriendlyId { get; }
        string FriendlyUrl { get; set; }
        IPageHtml ContentHtml { get; }          
        int Id { get; set; }
        bool IsPublished { get; set; }
        string Keywords { get; set; }
        int Layout { get; set; }
        int MenuOrder { get; set; }
        DateTime Modified { get; set; }
        string ModifiedBy { get; set; }
        NaviNode NaviNode { get; set; }
        string NaviTitle { get; set; }
        int Status { get; set; }
        string Title { get; set; }
        string Url { get; set; }
        ContentType Type { get; set; }
        bool Commentable { get; set; }
        bool Hidden { get; set; }
    }
}
