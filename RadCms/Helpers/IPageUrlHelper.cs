using RadCms.Entities;

namespace RadCms.Helpers
{
    public interface IPageUrlHelper
    {
        void UpdatePageUrl(CmsPage page);
        string GetBaseUrlFromNode(NaviNode node);
        void UpdatePageUrl(NaviNode node);
        string GetPageUrl(IPage page);
    }
}
