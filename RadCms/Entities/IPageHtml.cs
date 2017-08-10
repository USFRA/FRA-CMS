using System;
namespace RadCms.Entities
{
    public interface IPageHtml
    {
        string Summary { get; set; }
        string Content { get; set; }
        string Header { get; set; }
        string Sidebar { get; set; }
    }
}
