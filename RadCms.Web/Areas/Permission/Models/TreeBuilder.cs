using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RadCms.Models;
using RadCms.Entities;

namespace RadCms.Web.Areas.Permission.Models
{
    public class TreeBuilder
    {
        private List<NaviNode> _allNodes;

        public TreeBuilder(List<NaviNode> allNodes)
        {
            _allNodes = allNodes;
        }

        private string Indent(int level, string name)
        {
            StringBuilder sb = new StringBuilder();
            if (level == 1)
            {
                sb.Append("+ ");
            }
            else
            {
                for (int i = 0; i < level; i++)
                {
                    sb.Append("- ");
                }
            }

            sb.Append(name);

            return sb.ToString();
        }

        private void BuildTree(NaviNode e, int level, List<SelectListItem> items)
        {
            items.Add(new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = Indent(level, e.NodeName)
            });

            foreach (NaviNode n in _allNodes.Where(c => c.Parent != null && c.Parent.Id == e.Id))
            {
                BuildTree(n, level + 1, items);
            }
        }

        public IEnumerable<SelectListItem> BuildTree()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            NaviNode root = _allNodes.Single(e => e.Id == 1);
            BuildTree(root, 0, items);
            return items;
        }
    }
}
