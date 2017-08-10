using System.Web.Mvc;
using RadCms.Data;
using RadCms.Mvc;
using System;

namespace RadCms.Core.Containers.Controllers
{
    [Obsolete]
    public class PageController: PubControllerBase
    {
        private IDbContext _db;

        public PageController(IDbContext db)
        {
            _db = db;
        }

        public ActionResult Goto(int id)
        {
            return Redirect(PageHelper.Goto(id, _db));
        }
    }
}