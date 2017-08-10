using System;
using System.Collections.Generic;
using System.Linq;
using RadCms.Data;
using RadCms.Entities;
using RadCms.Models;

namespace RadCms.Services
{
    public class TreeModelService: ITreeModelService<TreeModel>
    {
        private IRepository<NaviNode> _naviNodeRepo;
        private IRepository<CmsPage> _cmsPageRepo;
        public TreeModelService(IRepository<NaviNode> naviNodeRepo, IRepository<CmsPage> cmsPageRepo)
        {
            this._cmsPageRepo = cmsPageRepo;
            this._naviNodeRepo = naviNodeRepo;
        }
        public IEnumerable<TreeModel> GetChildren(int parentId)
        {
            var folders = (from entity in _naviNodeRepo.GetAll().Where(x => x.Parent.Id == parentId)
                           select new TreeModel
                           {
                               Id = entity.Id,
                               Type = "N",
                               MenuOrder = entity.MenuOrder,
                               title = entity.NodeName,
                               hasChildren = true,
                               isLazy = false,
                               imageUrl = "/Core/assetsCms/images/folder.png",
                               expanded = false
                           }).ToList();
            foreach(var f in folders)
            {
                f.items = GetChildren(f.Id).ToList();
            }
            var pages = (from entity in _cmsPageRepo.GetAll().Where(x => x.NaviNode.Id == parentId)
                         select new TreeModel
                         {
                             Id = entity.Id,
                             Type = "P",
                             MenuOrder = entity.MenuOrder,
                             title = String.IsNullOrEmpty(entity.NaviTitle) ? entity.Title : entity.NaviTitle,
                             hasChildren = false,
                             isLazy = false,
                             linkUrl = "/" + entity.Url,
                             imageUrl = "/Core/assetsCms/images/page.png",
                             expanded = true,
                         }).ToList();
            return (folders.Union(pages)).OrderBy(a => a.MenuOrder);
        }

        public IEnumerable<TreeModel> GetChildrenFolders(int parentId)
        {
            var folders = (from entity in _naviNodeRepo.GetAll().Where(x => x.Parent.Id == parentId)
                           select new TreeModel
                           {
                               Id = entity.Id,
                               Type = "N",
                               MenuOrder = entity.MenuOrder,
                               title = entity.NodeName,
                               hasChildren = true,
                               isLazy = true,
                           }).ToList();
            return folders.OrderBy(a => a.MenuOrder);
        }
    }
}
