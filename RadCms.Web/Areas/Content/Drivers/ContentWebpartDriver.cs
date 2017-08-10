using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RadCms.Helpers;
using RadCms.Mvc.ViewEngines.Razor;

namespace RadCms.Web.Areas.Content.Drivers
{
    public class ContentWebpartDriver:IWebpartDriver
    {
        private DriverContext _context;
        public string WebpartId
        {
            get { return "CONTENT"; }
        }

        public void Apply(DriverContext context)
        {
            _context = context;
        }

        public DriverResult BuildDisplay()
        {
            return new DriverResult
            {
                Content = BuildContent(_context.ControllerContext, _context.Parameters)
            };
        }

        public DriverResult BuildEditor()
        {
            return BuildDisplay();
        }

        private static string BuildContent(ControllerContext context, string[] parameters)
        {
            var contentId = parameters.Length > 1 ? parameters[1].Trim() : "";

            if(String.IsNullOrEmpty(contentId))
            {
                return "";
            }

            return ViewRenderer.RenderViewToString(context, "~/Core/views/Content/" + contentId + ".cshtml", null, true);
        }

    }
}