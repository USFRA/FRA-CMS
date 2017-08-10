using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RadCms.Data;
using RadCms.Helpers;

namespace RadCms.Web.Areas.Comment.Drivers
{
    using Entities;
    using Models;
    using Mvc.ViewEngines.Razor;
    using RadCms.Entities;
    using System.Configuration;
    using System.Web.Configuration;

    public class CommentWebpartDriver : IWebpartDriver
    {
        private bool _skipRecaptcha;
        private IRepository<Comment> _repo;
        private const int ACTIVE = 1;
        private const int DELETED = 0;
        private const int PENDING = -1;

        public CommentWebpartDriver(IRepository<Comment> repo)
        {
            if(!bool.TryParse(WebConfigurationManager.AppSettings["RecaptchaSkipValidation"], out _skipRecaptcha))
            {
                _skipRecaptcha = false;
            }
                
            _repo = repo;
        }

        private static Dictionary<string, bool> statusMapping = new Dictionary<string, bool>(){
                        {"open", true},
                        {"on", true},
                        {"true", true},
                        {"false", false},
                        {"off", false},
                        {"closed", false},
                        {"close", false}
                    };
        private DriverContext _context;
        public string WebpartId
        {
            get { return "COMMENT"; }
        }

        public void Apply(DriverContext context)
        {
            _context = context;
        }

        public DriverResult BuildDisplay()
        {
            return new DriverResult
            {
                Content = BuildContent(
                    page: _context.Page, 
                    webpartHeaders: _context.Headers,
                    comments : _repo.GetAll().Where(c => c.PageId == _context.Page.Id && c.Status == ACTIVE).ToList(),
                    context : _context.ControllerContext, 
                    isEditState: false, 
                    isPublic: _context.IsPublic)
            };
        }

        public DriverResult BuildEditor()
        {
            return new DriverResult
            {
                Content = BuildContent(
                    page : _context.Page,
                    webpartHeaders : _context.Headers,
                    comments: _repo.GetAll().Where(c=>c.PageId == _context.Page.Id && c.Status == ACTIVE).ToList(),
                    context : _context.ControllerContext,
                    isEditState : true,
                    isPublic : _context.IsPublic)
            };
        }

        private string BuildContent(IPage page, List<Comment> comments, StringBuilder webpartHeaders, ControllerContext context, bool isEditState, bool isPublic)
        {
            var statusString = _context.Parameters.Length > 1 ? _context.Parameters[1].Trim() : "open";
            var status = statusMapping.ContainsKey(statusString) ? statusMapping[statusString] : true;

            if(!page.Commentable)
            {
                return "";
            }

            if(context == null)
            {
                var controller = ViewRenderer.CreateController<GenericController>();
                context = controller.ControllerContext;
            }
            else
            {
                if(context.Controller.TempData["ViewData"] != null)
                {
                    context.Controller.ViewData = (ViewDataDictionary)context.Controller.TempData["ViewData"];
                }
            }

            var account = context.HttpContext.User.Identity.Name.ToLower();
            var commentModel = new CommentViewModel
            {
                PageId = page.Id,
                Url = page.Url,
                Comments = comments,
                Status = status,
                IsEditState = isEditState,
                Email = GuessEmail(account),
                Name = GuessName(account).ToUpper()
            };

            context.Controller.ViewBag.RecaptchaSkipValidation = _skipRecaptcha;
            
            var contentString = ViewRenderer.RenderViewToString(context,
                "~/Areas/Comment/views/Comment/_CommentPartial.cshtml", commentModel, true);

            return contentString;
        }
       
        private string GuessName(string adAccount)
        {
            var tokens = adAccount.Replace("addot\\", "").Replace(".ctr", "").Split('.')
                .Where(t => !string.IsNullOrEmpty(t))
                .Select(t => t.Trim()).ToList();

            if(tokens.Count == 2)
            {
                return tokens[0] + " " + tokens[1];
            }
            else
            {
                return adAccount;
            }
        }

        private string GuessEmail(string adAccount)
        {
            var name = adAccount.Replace(CmsHelper.AdDomain + "\\", "");
            return name + "@" + CmsHelper.EmailDomain;
        }
    }
}