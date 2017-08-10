using System;
using System.Collections.Generic;
using System.Data.Entity;
using RadCms.Entities;
using RadCms.Helpers;
using System.Web;

namespace RadCms.Data
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<CmsContext>
    {
        protected override void Seed(CmsContext context)
        {
            string adName = HttpContext.Current.User.Identity.Name.ToLower();

            var currentUser = new
            {
                AdName = adName,
                Name = adName.Split('\\')[1]
            };

            var NOW = DateTime.Now;

            #region create Content Types
            ContentType t1 = new ContentType
            {
                Title = "PAGE"
            };

            context.Set<ContentType>().Add(t1);
            context.SaveChanges();
            #endregion

            #region create navi nodes
            NaviNode n = new NaviNode
            {
                NodeName = "Home",
                CreatedBy = currentUser.AdName,
                Created = NOW,
                ModifiedBy = currentUser.AdName,
                Modified = NOW,
                DefaultPageId = 1,
            };

            NaviNode n0 = new NaviNode
            {
                NodeName = "Explore",
                Parent = n,
                MenuOrder = 1,
                CreatedBy = currentUser.AdName,
                Created = NOW,
                ModifiedBy = currentUser.AdName,
                Modified = NOW,
            };

            NaviNode n1 = new NaviNode
            {
                NodeName = "Improve",
                Parent = n,
                MenuOrder = 1,
                CreatedBy = currentUser.AdName,
                Created = NOW,
                ModifiedBy = currentUser.AdName,
                Modified = NOW,
            };

            NaviNode n2 = new NaviNode
            {
                NodeName = "Balance",
                Parent = n,
                MenuOrder = 2,
                CreatedBy = currentUser.AdName,
                Created = NOW,
                ModifiedBy = currentUser.AdName,
                Modified = NOW,
            };

            NaviNode n3 = new NaviNode
            {
                NodeName = "Screen Yourself",
                Parent = n,
                MenuOrder = 3,
                CreatedBy = currentUser.AdName,
                Created = NOW,
                ModifiedBy = currentUser.AdName,
                Modified = NOW,
            };

            var naviNodes = new List<NaviNode> {
            n,
            n0,
            n1,
            n2,
            n3,
            };

            naviNodes.ForEach(s =>
            {
                s.Type = t1;
                context.Set<NaviNode>().Add(s);
            });
            context.SaveChanges();

            #endregion

            #region create layouts
            var level1 = new PageLayout
            {
                Style = @"
#cms-region-1 { 
    width: 100%; 
    min-height: 400px; 
    float: left; 
} 
#contentWrapper article{ 
    width: 100%; 
}
.image-group{
    border-top:1px solid grey;
    background-color: #EFF0F2;
    width: 100%; 
    height: 100%;
  }
.col.col-3-1, .col.col-3-2, .col.col-3-3{
    width: 33.3%;
    padding: 15px;
    box-sizing: border-box;
    float: left;
}",
                Title = "LEVEL 1",
                Template = @"
<div class=""main wrapper clearfix"">
    <article class=""clearfix"">
        [$webpart(breadcrumb)$]
        [$webpart(title)$]
        <div class=""cms-editable"" data-region=""region1"">
            {region1}
        </div>
    </article>
</div>
<div class=""cms-editable"" data-region=""region2"">
    <div class=""image-group clearfix"">
        <div class=""wrapper"">
            <div class=""col col-3-1""><img src=""""></div>
            <div class=""col col-3-2""><img src=""""></div>
            <div class=""col col-3-3""><img src=""""></div>
        </div>
    </div>
</div>"
            };
            var level2 = new PageLayout
            {
                Style = @"
#cms-region-1 { 
    width: 100%; 
} 
#cms-region-1 .cms-editable-container { 
    min-height: 400px; 
}",
                Title = "LEVEL 2",
                Template = @"
<div class=""main wrapper clearfix"">
    <article class=""clearfix"">
        [$webpart(breadcrumb)$]
        [$webpart(title)$]
        <div class=""cms-editable"" data-region=""region1"">
            {region1}
        </div>
    </article>
    [$webpart(sidemenu)$]
</div>"
            };
            var home = new PageLayout
            {
                Style = @"
#cms-region-1 { 
    min-height: 400px; 
    float: left; 
    width: 100%;
}
.main article{
    width: 100%;
}
",
                Title = "HOME",
                Template = @"
[$webpart(carousel)$]
<div class=""main wrapper clearfix"">
    <article class=""clearfix"">
        <div class=""cms-editable"" data-region=""region1"">
            {region1}
        </div>
    </article>
</div>
"
            };

            var layouts = new List<PageLayout>{
                level1,
                level2,
                home,
            };
            layouts.ForEach(s =>
            {
                s.Type = t1;
                context.Set<PageLayout>().Add(s);
            });
            context.SaveChanges();
            #endregion

            #region create pages
            var pages = new List<CmsPage> {
                new CmsPage{Title="Home", 
                    Html = new CmsPageHtml{Content=home.Template, Sidebar =""},
                    NaviNode = n,
                    IsPublished = true,
                    Layout = home.Id,
                    CreatedBy=currentUser.AdName, 
                    Status = CmsPage.STATUS_NORMAL,
                    Created= NOW,
                    ModifiedBy=currentUser.AdName,
                    Modified= NOW,
                },

                new CmsPage{Title="Overview", 
                    NaviNode = n0,
                    Html = new CmsPageHtml{Content=level1.Template, Sidebar =""},
                    IsPublished = true,
                    MenuOrder = 0,
                    Status = CmsPage.STATUS_NORMAL,
                    CreatedBy=currentUser.AdName, 
                    Created= NOW,
                    ModifiedBy=currentUser.AdName,
                    Modified= NOW,
                    Layout = level1.Id,
                },

                new CmsPage{Title="Overview", 
                    NaviNode = n1,
                    Html = new CmsPageHtml{Content=level1.Template, Sidebar =""},
                    IsPublished = true,
                    MenuOrder = 0,
                    Status = CmsPage.STATUS_NORMAL,
                    CreatedBy=currentUser.AdName, 
                    Created= NOW,
                    ModifiedBy=currentUser.AdName,
                    Modified= NOW,
                    Layout = level1.Id,
                },

                new CmsPage{Title="Overview", 
                    NaviNode = n2,
    
                    Html = new CmsPageHtml{Content=level1.Template, Sidebar =""},
                    IsPublished = true,
                    Status = CmsPage.STATUS_NORMAL,
                    CreatedBy=currentUser.AdName, 
                    Created= NOW,
                    ModifiedBy=currentUser.AdName,
                    Modified= NOW,
                    Layout = level1.Id,
                },

                new CmsPage{Title="Overview", 
                    NaviNode = n3,
    
                    Html = new CmsPageHtml{Content=level1.Template, Sidebar =""},
                    IsPublished = true,
                    Status = CmsPage.STATUS_NORMAL,
                    CreatedBy=currentUser.AdName, 
                    Created= NOW,
                    ModifiedBy=currentUser.AdName,
                    Modified= NOW,
                    Layout = level1.Id,
                },
                
            };
            IPageUrlHelper helper = new PageBasedUrlHelper();
            pages.ForEach(p => helper.UpdatePageUrl(p));
            pages.ForEach(s =>
            {
                s.Type = t1;
                context.Set<CmsPage>().Add(s);
            });
            context.SaveChanges();

            #endregion

            #region Cms Users

            var users = new List<CmsUser> {
                new CmsUser{
                    AdName = currentUser.AdName,
                    UserName = currentUser.Name,
                    RoleId = RoleType.Admin
                }
            };

            users.ForEach(s => context.Set<CmsUser>().Add(s));
            #endregion

            #region Footer
            var s1 = new FooterSection
            {
                Title = "CONTACT US",
                Column = 1,
                Order = 1,
            };
            var s2 = new FooterSection
            {
                Title = "CONNECT WITH US",
                Column = 2,
                Order = 1,
            };
            var s3 = new FooterSection
            {
                Title = "OPPORTUNITIES",
                Column = 3,
                Order = 1,
            };
            var s4 = new FooterSection
            {
                Title = "READERS & VIEWERS",
                Column = 4,
                Order = 1,
            };

            var sections = new List<FooterSection>
            {
                s1,s2,s3,s4,
            };
            var items = new List<FooterItem> {
                new FooterItem{
                    Section = s1,
                    Title = "Federal Railroad Administration<br />1200 New Jersey Avenue, SE<br />Washington, DC 20590",
                    Link = null,
                    Target = null,
                    CreatedBy = currentUser.AdName,
                    Created = NOW,
                    ModifiedBy = currentUser.AdName,
                    Modified = NOW,
                    IsPublished = true,
                    Index = 0
                },
                new FooterItem{
                    Section = s2,
                    Title = "Facebook",
                    Link = "http://www.facebook.com/USDOTFRA",
                    Target = "_blank",
                    CreatedBy = currentUser.AdName,
                    Created = NOW,
                    ModifiedBy = currentUser.AdName,
                    Modified = NOW,
                    IsPublished = true,
                    Index = 0
                },
                new FooterItem{
                    Section = s3,
                    Title = "Jobs",
                    Link = "#",
                    Target = "_self",
                    CreatedBy = currentUser.AdName,
                    Created = NOW,
                    ModifiedBy = currentUser.AdName,
                    Modified = NOW,
                    IsPublished = true,
                    Index = 0
                },
                new FooterItem{
                    Section = s4,
                    Title = "Adobe Acrobat Reader",
                    Link = "http://get.adobe.com/reader/",
                    Target = "_blank",
                    CreatedBy = currentUser.AdName,
                    Created = NOW,
                    ModifiedBy = currentUser.AdName,
                    Modified = NOW,
                    IsPublished = true,
                    Index = 0
                },
            };

            items.ForEach(s => context.Set<FooterItem>().Add(s));
            context.SaveChanges();

            #endregion

            foreach(CmsPage page in pages)
            {
                PublishDraft(page, context);
            }

            context.SaveChanges();

        }

        private void PublishDraft(CmsPage cmsPage, CmsContext db)
        {
            int innerId = cmsPage.Id;
            PubPage pubPage = new PubPage();
            pubPage.Id = cmsPage.Id;
            db.Set<PubPage>().Add(pubPage);

            pubPage.Title = cmsPage.Title;
            pubPage.NaviTitle = cmsPage.NaviTitle;
            pubPage.Layout = cmsPage.Layout;
            pubPage.MenuOrder = cmsPage.MenuOrder;

            if(pubPage.Html == null)
            {
                pubPage.Html = new PubPageHtml();
            }

            pubPage.Html.Content = cmsPage.Html.Content;
            pubPage.Html.Sidebar = cmsPage.Html.Sidebar;
            pubPage.Html.Header = cmsPage.Html.Header;

            pubPage.Description = cmsPage.Description;
            pubPage.Keywords = cmsPage.Keywords;
            pubPage.Layout = cmsPage.Layout;

            pubPage.NaviNode = cmsPage.NaviNode;

            pubPage.NaviTitle = cmsPage.NaviTitle;

            pubPage.Modified = cmsPage.Modified;
            pubPage.ModifiedBy = cmsPage.ModifiedBy;
            pubPage.Created = cmsPage.Created;
            pubPage.CreatedBy = cmsPage.CreatedBy;
            pubPage.Url = cmsPage.Url;
        }
    }
}
