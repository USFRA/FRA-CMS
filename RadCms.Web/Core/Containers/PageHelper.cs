using RadCms.Data;
using RadCms.Entities;
using System.Linq;

namespace RadCms.Core.Containers
{
    internal class PageHelper
    {
        internal static string Goto(int id, IDbContext db)
        {
            var url = db.Set<CmsPage>().Where(e => e.Id == id).Select(e => e.Url);
            return "/" + (url.Count() == 0 ? "" : url.First());
        }
    }
}
