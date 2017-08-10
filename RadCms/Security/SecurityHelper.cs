using System;
using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using RadCms.Entities;
using RadCms.Models;
using RadCms.Data;
using RadCms.Helpers;

namespace RadCms.Security
{
    class RolePermission
    {
        public string RoleName { get; set; }
        public int AccessMode { get; set; }
    }

    public class SecurityHelper
    {
        public static bool HasAccess()
        {
            return HasAccess2(new CmsContext());
        }

        public static PermissionType PageAccessMode(IDbContext db, CmsUser cmsUser, CmsPage page)
        {
            PermissionType mode = PermissionType.Default;

            if (cmsUser != null && cmsUser.RoleId >= RoleType.Super)
            {
                //supervisor
                mode = PermissionType.Admin;
            }
            else if (cmsUser != null && page != null)
            {
                PagePermission pp = db.Set<PagePermission>().SingleOrDefault(
                        e => e.User.Id == cmsUser.Id && e.Page.Id == page.Id);

                if (pp != null)
                {
                    mode = pp.AccessMode;
                }
                else
                {
                    mode = NaviAccessMode(db, cmsUser, page.NaviNode);
                }
            }

            // TODO: Add site specific access checking
            if(CmsHelper.Site == "fratalk" && page.Type.Title == "PAGE" && mode != PermissionType.Admin)
            {
                return PermissionType.Default;
            }

            return mode;
        }

        public static PermissionType PageAccessMode(IDbContext db, CmsPage page)
        {
            return PageAccessMode(db, CurrentCmsUser(db), page);
        }

        public static PermissionType NaviAccessMode(IDbContext db, CmsUser cmsUser, NaviNode navi)
        {
            PermissionType mode = PermissionType.Default;

            if (cmsUser != null && cmsUser.RoleId >= RoleType.Super)
            {
                mode = PermissionType.Admin;
            }
            else if (cmsUser != null && navi != null)
            {
                NaviNode currentNode = navi;

                while (currentNode != null)
                {
                    NaviPermission pp = db.Set<NaviPermission>().SingleOrDefault(
                            e => e.User.Id == cmsUser.Id && e.Section.Id == currentNode.Id);

                    if (pp != null)
                    {
                        mode = pp.AccessMode;

                        break;
                    }

                    currentNode = currentNode.Parent;
                }
            }

            return mode;
        }

        public static PermissionType NaviAccessMode(IDbContext db, NaviNode navi)
        {
            return NaviAccessMode(db, CurrentCmsUser(db), navi);
        }

        public static PermissionType TargetAccessMode(IDbContext db, CmsUser cmsUser, string target)
        {
            PermissionType mode = 0;

            if (cmsUser != null && cmsUser.RoleId >= RoleType.Super)
            {
                mode = PermissionType.Admin;
            }
            else if (cmsUser != null && target != null)
            {
                Permission p = db.Set<Permission>().SingleOrDefault(e => e.User.Id == cmsUser.Id
                    && e.Target == target);

                if (p != null)
                {
                    mode = p.AccessMode;
                }
            }

            return mode;
        }

        public static CmsUser CurrentCmsUser(IDbContext db)
        {
            CmsUser cmsUser = null;

            if (HttpContext.Current.User != null)
            {
                IIdentity WinId = HttpContext.Current.User.Identity;
                WindowsIdentity wi = (WindowsIdentity)WinId;

                string accountName = wi.Name.ToLower();

                cmsUser = db.Set<CmsUser>().SingleOrDefault(e => e.AdName == accountName);
            }

            return cmsUser;
        }

        public static RoleType CurrentCmsUserRole(IDbContext db)
        {
            RoleType role = RoleType.Normal;

            CmsUser cmsUser = CurrentCmsUser(db);

            if (cmsUser != null)
            {
                role = cmsUser.RoleId;
            }

            return role;
        }

        public static bool HasAccess2(IDbContext db)
        {
            return CurrentCmsUser(db) != null;

            //bool isAuthorized = false;

            //if (HttpContext.Current.User != null)
            //{
            //    IIdentity WinId = HttpContext.Current.User.Identity;
            //    WindowsIdentity wi = (WindowsIdentity)WinId;

            //    string accountName = wi.Name.ToLower();

            //    CmsUser cmsUser = db.CmsUsers.SingleOrDefault(e => e.Id == accountName);

            //    isAuthorized = cmsUser != null;

            //not by group
            //if (!isAuthorized)
            //{
            //    //HashSet<string> groupNames = new HashSet<string>();

            //    if (!wi.IsAnonymous && wi.IsAuthenticated)
            //    {
            //        WindowsPrincipal user = new WindowsPrincipal(wi);

            //        IdentityReferenceCollection irc = wi.Groups;
            //        foreach (IdentityReference ir in irc)
            //        {
            //            //groupNames.Add(ir.Translate(typeof(NTAccount)).Value);

            //            isAuthorized = permissionSet.Contains(ir.Translate(typeof(NTAccount)).Value);

            //            if (isAuthorized)
            //            {
            //                break;
            //            }
            //        }
            //    }
            //}
            //}

            //return isAuthorized;
        }

        public static bool IsAdmin()
        {
            return IsAdmin(CurrentCmsUser(new CmsContext()));
        }

        public static bool IsAdmin(CmsUser cmsUser)
        {
            return cmsUser != null && cmsUser.RoleId > RoleType.Super;
        }

        public static bool IsSuperUser()
        {
            return IsSuperUser(CurrentCmsUser(new CmsContext()));
        }

        public static bool IsSuperUser(CmsUser cmsUser)
        {
            bool isSuperUser = false;

            isSuperUser = cmsUser != null && cmsUser.RoleId >= RoleType.Super;

            //return isSuperUser;

            //if (HttpContext.Current.User != null)
            //{
            //    IIdentity WinId = HttpContext.Current.User.Identity;
            //    WindowsIdentity wi = (WindowsIdentity)WinId;

            //    string accountName = wi.Name.ToLower();

            //    CmsUser cmsUser = db.CmsUsers.SingleOrDefault(e => e.Id == accountName);

            //    isSuperUser = cmsUser != null && cmsUser.RoleId >= 1;

            //if (!isSuperUser)
            //{
            //    //HashSet<string> groupNames = new HashSet<string>();

            //    if (!wi.IsAnonymous && wi.IsAuthenticated)
            //    {
            //        WindowsPrincipal user = new WindowsPrincipal(wi);

            //        IdentityReferenceCollection irc = wi.Groups;
            //        foreach (IdentityReference ir in irc)
            //        {
            //            //groupNames.Add(ir.Translate(typeof(NTAccount)).Value);

            //            isSuperUser = superuserSet.Contains(ir.Translate(typeof(NTAccount)).Value);

            //            if (isSuperUser)
            //            {
            //                break;
            //            }
            //        }
            //    }
            //}
            //}

            return isSuperUser;
        }
    }
}