using System.Web.Mvc;
using RadCms.Services;
using RadCms.Mvc;

namespace RadCms.Web.Areas.Template.Controllers
{
    public class LayoutController : PubControllerBase
    {
        private ILayoutService _layoutService;

        public LayoutController(ILayoutService service)
        {
            _layoutService = service;
        }

        public ContentResult Styles(int id)
        {
            var layout = _layoutService.Find(id);
            
            if (layout == null)
            {
                //there is no default page layout set
                return null;
            }

            return Content(layout.Style, "text/css");
        }
    }
}