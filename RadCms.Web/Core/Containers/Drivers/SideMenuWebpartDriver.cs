using System.Web.Mvc;
using RadCms.Entities;
using RadCms.Helpers;
using RadCms.Models;
namespace RadCms.Core.Containers.Drivers
{
    public class SideMenuWebpartDriver: IWebpartDriver
    {
        private DriverContext _context;
        private IPageUrlHelper _urlHelper;

        public SideMenuWebpartDriver(IPageUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string WebpartId
        {
            get { return "SIDEMENU"; }
        }

        public void Apply(DriverContext context)
        {
            _context = context;
        }

        public DriverResult BuildDisplay()
        {
            return new DriverResult
            {
                Content = BuildContent(_context.Page.Id, _context.Page.NaviNode, _context.IsPublic)
            };
        }

        public DriverResult BuildEditor()
        {
            return BuildDisplay();
        }

        private string BuildContent(int pageId, NaviNode pageNode, bool isPublic)
        {
            var menuBuilder = new MenuBuilder(pageId, pageNode, _urlHelper);
            menuBuilder.IsPublic = isPublic;
            var asideBuilder = new TagBuilder("div");
            asideBuilder.AddCssClass("cms-replaceable");
            asideBuilder.Attributes.Add("data-replace", "[$webpart(sidemenu)$]");
            asideBuilder.InnerHtml = menuBuilder.ToHtmlString();
            return asideBuilder.ToString();
        }
    }
}