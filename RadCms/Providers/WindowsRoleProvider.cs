using RadCms.Data;
using RadCms.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Security;

namespace RadCms.Providers
{
    public class WindowsRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                return this.GetType().Assembly.GetName().Name.ToString();
            }
            set
            {
                this.ApplicationName = this.GetType().Assembly.GetName().Name.ToString();
            }
        }

        public override bool RoleExists(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }
            using (CmsContext Context = new CmsContext())
            {
                WebRole Role = null;
                Role = Context.Set<WebRole>().FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (Role != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }
            using (CmsContext Context = new CmsContext())
            {
                WebUser User = null;
                User = Context.Set<WebUser>().FirstOrDefault(Usr => Usr.AdName == username);
                if (User == null)
                {
                    return false;
                }
                WebRole Role = Context.Set<WebRole>().FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (Role == null)
                {
                    return false;
                }
                return User.Roles.Contains(Role);
            }
        }

        public override string[] GetAllRoles()
        {
            using (CmsContext Context = new CmsContext())
            {
                return Context.Set<WebRole>().Select(Rl => Rl.RoleName).ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return new string[] { }; 
            }
            using (CmsContext Context = new CmsContext())
            {
                WebRole Role = null;
                Role = Context.Set<WebRole>().FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (Role != null)
                {
                    return Role.Users.Select(Usr => Usr.AdName).ToArray();
                }
                else
                {
                    return new string[] { }; 
                }
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new string[] { }; 
            }
            using (CmsContext Context = new CmsContext())
            {
                WebUser User = null;
                User = Context.Set<WebUser>().FirstOrDefault(Usr => Usr.AdName == username);
                if (User != null && User.Roles !=null)
                {
                    return User.Roles.Select(Rl => Rl.RoleName).ToArray();
                }
                else
                {
                    return new string[]{};
                }
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return new string[] { }; 
            }

            if (string.IsNullOrEmpty(usernameToMatch))
            {
                return new string[] { }; 
            }

            using (CmsContext Context = new CmsContext())
            {

                return (from Rl in Context.Set<WebRole>() from Usr in Rl.Users where Rl.RoleName == roleName && Usr.AdName.Contains(usernameToMatch) select Usr.AdName).ToArray();
            }
        }

        public override void CreateRole(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                using (CmsContext Context = new CmsContext())
                {
                    WebRole Role = null;
                    Role = Context.Set<WebRole>().FirstOrDefault(Rl => Rl.RoleName == roleName);
                    if (Role == null)
                    {
                        WebRole NewRole = new WebRole
                        {
                            RoleName = roleName
                        };
                        Context.Set<WebRole>().Add(NewRole);
                        Context.SaveChanges();
                    }
                }
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }
            using (CmsContext Context = new CmsContext())
            {
                WebRole Role = null;
                Role = Context.Set<WebRole>().FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (Role == null)
                {
                    return false;
                }
                if (throwOnPopulatedRole)
                {
                    if (Role.Users.Any())
                    {
                        return false;
                    }
                }
                else
                {
                    Role.Users.Clear();
                }
                Context.Set<WebRole>().Remove(Role);
                Context.SaveChanges();
                return true;
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (CmsContext Context = new CmsContext())
            {
                List<WebUser> Users = Context.Set<WebUser>().Where(Usr => usernames.Contains(Usr.AdName)).ToList();
                List<WebRole> Roles = Context.Set<WebRole>().Where(Rl => roleNames.Contains(Rl.RoleName)).ToList();
                foreach (WebUser user in Users)
                {
                    foreach (WebRole role in Roles)
                    {
                        if (!user.Roles.Contains(role))
                        {
                            user.Roles.Add(role);
                        }
                    }
                }
                Context.SaveChanges();
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (CmsContext Context = new CmsContext())
            {
                foreach (String username in usernames)
                {
                    String us = username;
                    WebUser user = Context.Set<WebUser>().FirstOrDefault(U => U.AdName == us);
                    if (user != null)
                    {
                        foreach (String roleName in roleNames)
                        {
                            String rl = roleName;
                            WebRole role = user.Roles.FirstOrDefault(R => R.RoleName == rl);
                            if (role != null)
                            {
                                user.Roles.Remove(role);
                            }
                        }
                    }
                }
                Context.SaveChanges();
            }
        }
    }
}