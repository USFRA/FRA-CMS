using System.Collections.Generic;
using System.Linq;
using RadCms.Data;
using RadCms.Entities;
using System.Data;
using RadCms.Security;

namespace RadCms.Services
{
    public class LayoutService: ILayoutService
    {
        private IRepository<PageLayout> _layoutRepo;

        public LayoutService(IRepository<PageLayout> layoutRepo)
        {
            _layoutRepo = layoutRepo;
        }

        public IEnumerable<PageLayout> GetList()
        {
            var query = _layoutRepo.GetAll();

            if (!SecurityHelper.IsAdmin())
            {
                query = query.Where(e => e.IsVisible);
            }
            return query.OrderBy(e => e.Order).ToList();
        }

        public IEnumerable<PageLayout> GetList(string sectionType)
        {
            var query = _layoutRepo.GetAll().Where(e => e.Type.Title == sectionType);

            if (!SecurityHelper.IsAdmin())
            {
                query = query.Where(e => e.IsVisible);
            }

            return query.OrderBy(e => e.Order).ToList();
        }

        public PageLayout Find(int id)
        {
            var layout = _layoutRepo.Get(id);
            if (layout == null)
            {
                layout = _layoutRepo.GetAll().First();
            }

            return layout;
        }

        public void AddLayout(PageLayout layout)
        {
            _layoutRepo.Add(layout);
            _layoutRepo.Save();
        }

        public void UpdateLayout(PageLayout layout)
        {
            var old = Find(layout.Id);
            old.Image = layout.Image;
            old.IsVisible = layout.IsVisible;
            old.Order = layout.Order;
            old.Style = layout.Style;
            old.Template = layout.Template;
            old.Title = layout.Title;
            //_layoutRepo.Update(layout);

            _layoutRepo.Save();
        }

        public void DeleteLayout(int id)
        {
            PageLayout pagelayout = _layoutRepo.Get(id);
            _layoutRepo.Delete(pagelayout);
            _layoutRepo.Save();
        }
    }
}
