using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadCms.Entities;
using System.Data.Entity;
using RadCms.Data;

namespace RadCms.Services
{
    public class PageModelBuilder<T> where T: class, IPage
    {
        private IDbContext _dbContext;

        public PageModelBuilder(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetPage(string id)
        {
            T page;
            if(!CmsPage.IsFriendlyId(id))
            {
                page = _dbContext.Set<T>().SingleOrDefault(e => e.Url.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                var innerId = CmsPage.FromFriendlyId(id);
                page = _dbContext.Set<T>().SingleOrDefault(e => e.Id == innerId);
            }

            return page;
        }
    }
}
