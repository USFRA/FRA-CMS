using System.Web.Mvc;

namespace RadCms.Mvc.ViewEngines.Razor
{
    public class RadCmsRazorViewEngine : RazorViewEngine
    {
        // Custom area locations
        public RadCmsRazorViewEngine()
        {
            var viewFormats = new[]{
                "~/Core/Views/{1}/{0}.cshtml",
                "~/Core/Views/Shared/{1}/{0}.cshtml",
                "~/Areas/Views/{1}/{0}.cshtml",
                "~/Areas/Views/Shared/{1}/{0}.cshtml",
                "~/Themes/Views/{1}/{0}.cshtml",
                "~/Themes/Views/Shared/{1}/{0}.cshtml",
            };

            var areaFormats = new[]{
                "~/Core/{2}/Views/{1}/{0}.cshtml",
                "~/Core/{2}/Views/Shared/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{1}/{0}.cshtml",
                "~/Themes/{2}/Views/{1}/{0}.cshtml",
                "~/Themes/{2}/Views/Shared/{1}/{0}.cshtml",
            };

            ViewLocationFormats = viewFormats;
            MasterLocationFormats = viewFormats;
            PartialViewLocationFormats = viewFormats;

            AreaViewLocationFormats = areaFormats;
            AreaMasterLocationFormats = areaFormats;
            AreaPartialViewLocationFormats = areaFormats;
        }
    }
}
