using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadCms.Models;
using RadCms.Entities;
using RadCms.Data;

namespace RadCms.Security
{
    public class WebSecurityHelpers
    {
        public static DateTime? GetLastSubmitTime(string ip)
        {
            using (CmsContext db = new CmsContext())
            {
                var last = db.Set<FormSubmitRequest>().SingleOrDefault(e => e.ClientIP == ip);
                if (last != null) 
                {
                    return last.LastSubmit;
                }
            }
            return null;
        }

        public static void UpdateOrCreateSubmitTime(string ip)
        {
            using (CmsContext db = new CmsContext())
            { 
                var now = DateTime.Now;
                var last = db.Set<FormSubmitRequest>().SingleOrDefault(e => e.ClientIP == ip);
                if (last != null)
                {
                    last.LastSubmit = now;
                }
                else
                {
                    last = new FormSubmitRequest
                    {
                        ClientIP = ip,
                        LastSubmit = now
                    };

                    db.Set<FormSubmitRequest>().Add(last);
                    db.SaveChanges();
                }
            }
        }
    }
}
