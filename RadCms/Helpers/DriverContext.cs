using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RadCms.Entities;

namespace RadCms.Helpers
{
    public class DriverContext
    {
        public bool IsPublic { get; set; }
        public string WebpartId { get; set; }
        public StringBuilder Headers { get; set; }
        public ControllerContext ControllerContext { get; set; }
        public string[] Parameters { get; set; }
        public IPage Page { get; set; }
    }
}
