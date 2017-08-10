
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RadCms.Entities;
using RadCms.Helpers;

namespace RadCms.Core.Containers.Drivers
{
    public class TitleWebpartDriver:IWebpartDriver
    {
        private DriverContext _context;
        public string WebpartId
        {
            get { return "TITLE"; }
        }

        public void Apply(DriverContext context)
        {
            _context = context;
        }

        public DriverResult BuildDisplay()
        {
            return new DriverResult{
                Content = BuildContent(_context.Page)
            };
        }

        public DriverResult BuildEditor()
        {
            return BuildDisplay();
        }

        private static string BuildContent(IPage page)
        {
            return "<h1 data-replace=\"[$webpart(title)$]\" class=\"cms-replaceable\">" + (page == null ? "[Page Title]" : page.Title) + "</h1>";
        }

    }
}