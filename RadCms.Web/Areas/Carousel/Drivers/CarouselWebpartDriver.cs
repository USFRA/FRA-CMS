using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RadCms.Data;
using RadCms.Helpers;

namespace RadCms.Web.Areas.Carousel.Drivers
{
    using RadCms.Entities;

    public class CarouselWebpartDriver: IWebpartDriver
    {
        private DriverContext _context;
        private IRepository<Carousel> _repo;

        public CarouselWebpartDriver(IRepository<Carousel> repo)
        {
            _repo = repo;
        }

        public string WebpartId
        {
            get
            {
                return "CAROUSEL";
            }
        }

        public void Apply(DriverContext context)
        {
            _context = context;
        }

        public DriverResult BuildDisplay()
        {
            var slides = _repo.GetAll().Where(e => e.Visible).OrderBy(e => e.SlideId).ToList();
            return new DriverResult
            {
                Content = BuildContent(slides, _context)
            };
        }

        public DriverResult BuildEditor()
        {
            return BuildDisplay();
        }

        private static string BuildContent(List<Carousel> slides, DriverContext context)
        {
            var sb = new StringBuilder();
            var stylesheet = new TagBuilder("link");
            stylesheet.Attributes.Add("href", UrlHelper.GenerateContentUrl("~/Areas/Carousel/assets/style.css", context.ControllerContext.HttpContext));
            stylesheet.Attributes.Add("rel", "stylesheet");
            stylesheet.Attributes.Add("type", "text/css");

            var script = new TagBuilder("script");
            script.Attributes.Add("src", UrlHelper.GenerateContentUrl("~/Areas/Carousel/assets/script.js", context.ControllerContext.HttpContext));
            script.Attributes.Add("type", "text/javascript");

            context.Headers.AppendLine(stylesheet.ToString(TagRenderMode.SelfClosing));
            context.Headers.AppendLine(script.ToString(TagRenderMode.Normal));

            sb.Append("<div data-replace=\"[$webpart(carousel)$]\" id=\"carouselWrapper\" webpartId=\"carousel\" class=\"webpart cms-replaceable\">");

            for(int i = 0; i < slides.Count(); i++)
            {
                var item = slides[i];
                sb.Append("<div class=\"carouselItem\" data-id=\"");
                sb.Append(i + 1);
                sb.Append("\"");
                if(i == 0)
                {
                    sb.Append(" style=\"left:0px;\"");
                }
                sb.Append(">");
                sb.Append(item.SlideContent);
                sb.Append("</div>");
            }
            sb.Append("<ul class=\"nodes\">");
            for(int i = 0; i < slides.Count(); i++)
            {
                sb.Append("<li class=\"node\" data-id=\"");
                sb.Append(i + 1);
                sb.Append("\"></li>");
            }
            sb.Append("</ul>");
            sb.Append("</div>");

            return sb.ToString();
        }
    }
}