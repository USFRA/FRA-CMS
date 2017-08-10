using RadCms.Entities;
using System.Collections.Generic;

namespace RadCms.Services
{
    public interface ILayoutService
    {
        IEnumerable<PageLayout> GetList();
        IEnumerable<PageLayout> GetList(string sectionType);

        PageLayout Find(int id);

        void AddLayout(PageLayout pagelayout);

        void UpdateLayout(PageLayout pagelayout);

        void DeleteLayout(int id);
    }
}