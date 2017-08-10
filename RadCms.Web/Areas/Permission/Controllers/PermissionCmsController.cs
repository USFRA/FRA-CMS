using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using RadCms.Helpers;
using RadCms.Web.Areas.Permission.Models;
using RadCms.Security;

namespace RadCms.Web.Areas.Permission.Controllers
{
    using Data;
    using Entities;
    using Mvc;

    public class PermissionCmsController : CmsControllerBase
    {
        private IDbContext db;
        
        public PermissionCmsController(IDbContext db)
        {
            this.db = db;
        }

        readonly IDictionary<RoleType, string> roles = new Dictionary<RoleType, string>()
        { 
            {RoleType.Normal, "Normal"},
            {RoleType.Super, "Super"},
            {RoleType.Admin, "Admin"}
        };

        private IEnumerable<SelectListItem> UserRoles
        {
            get
            {
                return new SelectList(roles, "Key", "Value");
            }
        }

        readonly IDictionary<PermissionType, string> modes = new Dictionary<PermissionType, string>()
        {
            {PermissionType.Default, "Read"},
            //{PermissionType.Webpart, "Webpart"},
            {PermissionType.Edit, "Edit"},
            {PermissionType.Publish, "Publish"},
            {PermissionType.Create, "Create"},
            {PermissionType.Denied, "- Remove -"},
        };

        private IEnumerable<SelectListItem> AccessModes
        {
            get
            {
                return new SelectList(modes, "Key", "Value");
            }
        }

        public ViewResult Manage(string id, string sectionId, string adName)
        {
            var allNodes = db.Set<NaviNode>().OrderBy(e => e.NodeName).ToList();

            TreeBuilder tb = new TreeBuilder(allNodes);

            IEnumerable<SelectListItem> sections = tb.BuildTree();

            ViewBag.AccessModes = AccessModes;

            IEnumerable<SelectListItem> users = db.Set<CmsUser>().OrderBy(e => e.UserName)
                            .ToList().Select(e => new SelectListItem
                            {
                                Value = e.AdName,
                                Text = e.UserName
                            });

            ViewBag.Users = users;

            //pre-selected information
            if (!String.IsNullOrWhiteSpace(sectionId))
            {
                int sid = Convert.ToInt32(sectionId);
                NaviNode n = db.Set<NaviNode>().SingleOrDefault(e => e.Id == sid);

                if (n != null)
                {
                    ViewBag.SectionId = sid;
                }
            }

            //current user role
            if (!String.IsNullOrWhiteSpace(id))
            {
                int pageId = Convert.ToInt32(id);
                CmsPage p = db.Set<CmsPage>().SingleOrDefault(e => e.Id == pageId);

                if (p != null)
                {
                    ViewBag.SectionId = p.NaviNode.Id;
                    ViewBag.PageId = pageId;
                }
            }

            if (!String.IsNullOrWhiteSpace(adName))
            {
                CmsUser u = db.Set<CmsUser>().SingleOrDefault(e => e.AdName == adName);

                if (u != null)
                    ViewBag.AdName = adName;
            }

            ViewBag.RoleId = SecurityHelper.CurrentCmsUserRole(db);
            return View(sections);
        }

        public JsonResult GetPages(string sectionId)
        {
            int sid = Convert.ToInt32(sectionId);
            var pages = db.Set<CmsPage>().Where(e => e.NaviNode.Id == sid).ToList().Select(e => new SelectListItem
                                        {
                                            Value = e.Id.ToString(),
                                            Text = e.Title,
                                            Selected = false
                                        });
            return Json(new
            {
                Pages = pages
            });
        }

        public ViewResult UserPermissions(string u)
        {
            ViewBag.RoleId = SecurityHelper.CurrentCmsUserRole(db);
            CmsUser user = db.Set<CmsUser>().SingleOrDefault(e => e.AdName == u);
            if (user == null)
            {
                ViewBag.Message = "User does not exist.";
                return View("error");
            }
            List<PagePermission> pp = db.Set<PagePermission>().Where(e => e.User.AdName == u).ToList();
            List<NaviPermission> np = db.Set<NaviPermission>().Where(e => e.User.AdName == u).ToList();
            ViewBag.Page = pp;
            ViewBag.Navi = np;
            ViewBag.User = user;
            return View(user);
        }

        private bool IsN1UnderN2(NaviNode n1, NaviNode n2)
        {
            bool result = false;

            NaviNode current = n1.Parent;
            while (current != null)
            {
                if (current.Id == n2.Id)
                {
                    result = true;
                    break;
                }
                current = current.Parent;
            }

            return result;
        }

        private bool IsPUnderN(CmsPage p, NaviNode n2)
        {
            bool result = false;

            NaviNode current = p.NaviNode;
            while (current != null)
            {
                if (current.Id == n2.Id)
                {
                    result = true;
                    break;
                }
                current = current.Parent;
            }

            return result;
        }

        private void ClearPermissions(CmsUser user)
        {
            List<NaviPermission> naviPermissions =
                      db.Set<NaviPermission>().Where(e => e.User.Id == user.Id).ToList();

            foreach (NaviPermission np in naviPermissions)
            {
                db.Set<NaviPermission>().Remove(np);
            }

            List<PagePermission> PagePermissions =
                    db.Set<PagePermission>().Where(e => e.User.Id == user.Id).ToList();

            foreach (PagePermission pp in PagePermissions)
            {
                db.Set<PagePermission>().Remove(pp);
            }
        }

        [HttpPost]
        public JsonResult ChangeAccess(int userId, string action, string canLib, string isAdmin)
        {
            CmsUser currentUser = SecurityHelper.CurrentCmsUser(db);

            if (currentUser.RoleId < RoleType.Super)
            {
                throw new Exception("Access Denided.");
            }

            CmsUser user = db.Set<CmsUser>().SingleOrDefault(e => e.Id == userId);

            if (user != null)
            {
                switch (action)
                {
                    case "setAccess":
                        // full elib access 4
                        user.RoleId = isAdmin == "1" ? RoleType.Admin : RoleType.Normal;

                        ((DbContext)db).Entry(user).State = EntityState.Modified;
                        
                        /*
                        Permission p = db.Permission.SingleOrDefault(
                            e => e.User.Id == userId && e.Target == "LIB");

                        if (p != null)
                        {
                            p.AccessMode = canLib == "1" ? 1 : 0;
                            db.Entry(p).State = EntityState.Modified;
                        }
                        else
                        {
                            Permission permission = new Permission()
                            {
                                User = user,
                                Target = "LIB",
                                AccessMode = canLib == "1" ? 1 : 0
                            };
                            db.Permission.Add(permission);
                        }*/

                        db.SaveChanges();

                        break;

                    case "removeAccess":
                        ClearPermissions(user);
                        db.Set<CmsUser>().Remove(user);
                        db.SaveChanges();
                        break;

                    case "clearAccess":
                        ClearPermissions(user);
                        db.SaveChanges();
                        break;
                }
            }

            return Json(new
            {
                Result = "Success"
            });
        }
        
        [HttpPost]
        public ViewResult Change(string sectionId, string pageId, string userName, PermissionType permission, bool overwrite = false)
        {
            CmsUser currentUser = SecurityHelper.CurrentCmsUser(db);

            if (currentUser.RoleId < RoleType.Super)
            {
                throw new Exception("Access Denided.");
            }

            //user
            if (string.IsNullOrEmpty(userName))
            {
                ViewBag.Message = "User Name is Required";
                return View("error");
            }

            userName = userName.ToLower();

            CmsUser user = db.Set<CmsUser>().SingleOrDefault(e => e.AdName == userName);

            if (user == null && permission != PermissionType.Denied)
            {
                //new user
                user = new CmsUser();
                user.AdName = userName;
                user.UserName = HtmlHelpers.FormatName(null, userName).ToString();
                user.RoleId = RoleType.Normal;

                db.Set<CmsUser>().Add(user);
                db.SaveChanges();
            }
            else if (user != null)
            {
                if (permission == PermissionType.Denied)
                {
                    //remove users
                    ClearPermissions(user);

                    db.Set<CmsUser>().Remove(user);

                    db.SaveChanges();
                }
                else
                {
                    if (string.IsNullOrEmpty(pageId))
                    {
                        //navi
                        int sid = Convert.ToInt32(sectionId);

                        //handle overwrite

                        NaviNode currentNode = db.Set<NaviNode>().Single(e => e.Id == sid);

                        if (overwrite == true)
                        {
                            ClearPermissions(user);
                        }

                        NaviPermission np = db.Set<NaviPermission>().SingleOrDefault(e => e.Section.Id == sid && e.User.Id == user.Id);

                        //new navi permission
                        if (np == null)
                        {
                            np = new NaviPermission();
                            np.User = user;
                            np.AccessMode = permission;
                            np.Section = db.Set<NaviNode>().Single(e => e.Id == sid);
                            db.Set<NaviPermission>().Add(np);
                        }
                        //modify
                        else if (np != null)
                        {
                            if (permission != np.AccessMode)
                            {
                                np.AccessMode = permission;
                                ((DbContext)db).Entry(np).State = EntityState.Modified;
                            }
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        //page
                        int pid = Convert.ToInt32(pageId);
                        PagePermission pp = db.Set<PagePermission>().SingleOrDefault(e => e.Page.Id == pid && e.User.Id == user.Id);

                        //new page permission
                        if (pp == null)
                        {
                            pp = new PagePermission();
                            pp.User = user;
                            pp.AccessMode = permission;
                            pp.Page = db.Set<CmsPage>().Single(e => e.Id == pid);
                            db.Set<PagePermission>().Add(pp);
                        }
                        //modify
                        else if (pp != null)
                        {
                            if (permission != pp.AccessMode)
                            {
                                pp.AccessMode = permission;
                                ((DbContext)db).Entry(pp).State = EntityState.Modified;
                            }
                        }

                        db.SaveChanges();
                    }

                }
            }

            ViewBag.RoleId = SecurityHelper.CurrentCmsUserRole(db);

            return View("PermissionGranted");
        }
        
        [HttpGet]
        public ActionResult NaviPermission(string id)
        {
            int sid = Convert.ToInt32(id);
            var users = db.Set<CmsUser>().ToList();
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (CmsUser u in users)
            {
                PermissionType accessMode = SecurityHelper.NaviAccessMode(db, u, db.Set<NaviNode>().Single(e => e.Id == sid));
                if (accessMode > 0)
                {
                    if (accessMode == PermissionType.Admin)
                    {
                        result.Add(u.AdName, "Admin");
                    }
                    else
                    {
                        result.Add(u.AdName, modes[accessMode]);
                    }
                }
            }
            ViewBag.Permissions = result;
            return View("Permission");
        }

        public JsonResult getPermissions(string id)
        {
            int sid = Convert.ToInt32(id);
            var users = db.Set<CmsUser>().ToList();
            var naviPermissions = db.Set<NaviPermission>().Select(e => new PermissionModel
            {
                NodeName = e.Section.NodeName,
                User = e.User.UserName,
                AccessMode = e.AccessMode
            }).ToList();
            var pagePermissions = db.Set<PagePermission>().Select(e => new PermissionModel
            {
                NodeName = e.Page.Title,
                User = e.User.UserName,
                AccessMode = e.AccessMode
            }).ToList();
            var permissions = naviPermissions.Concat(pagePermissions);
            return Json(new
            {
                permissions = naviPermissions
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PagePermission(string id)
        {
            int pid = Convert.ToInt32(id);
            var users = db.Set<CmsUser>().ToList();
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (CmsUser u in users)
            {
                PermissionType accessMode = SecurityHelper.PageAccessMode(db, u, db.Set<CmsPage>().Single(e => e.Id == pid));
                if (accessMode > 0)
                {
                    if (accessMode == PermissionType.Admin)
                    {
                        result.Add(u.AdName, "Admin");
                    }
                    else
                    {
                        result.Add(u.AdName, modes[accessMode]);
                    }
                }
            }
            ViewBag.Permissions = result;
            return View("Permission");
        }
    }
}
