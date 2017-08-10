using System.Text;
using System.Web.Mvc;
using RadCms.Entities;
namespace RadCms.Helpers
{
    public interface IPageEngine
    {
        string ReplaceTokens(IPage page, StringBuilder webpartHeaders, out bool havingWebPart, ControllerContext controllerContext = null, bool isEditState = false);
    }
}