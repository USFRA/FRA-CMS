using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using RadCms.Entities;

namespace RadCms.Models
{
    public class ViewPageModel
    {
        public CmsPage Page { get; set; }

        public IEnumerable<HrefLink> Breadcrumb { get; set; }

        public IEnumerable<HrefLink> LeftNavi { get; set; }
    }
}