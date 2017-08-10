using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web.Mvc;
using System.Text;

using RadCms.Entities;
using Newtonsoft.Json;
using RadCms.Data;

namespace RadCms.Helpers
{
    public class PageEngine : IPageEngine
    {
        private IDbContext _db;
        private IDriverCoordinator _coordinator;
        // Injected by container
        public bool IsPublic { private get; set; }

        public PageEngine(IDbContext db, IDriverCoordinator coordinator)
        {
            _db = db;
            _coordinator = coordinator;
        }

        private static Regex functionRegex = new Regex(
                      @"\[\$(?<functionName>[^\$]*?)\(" +
                      @"(?:(?<params>.*?)(?:,|(?=\))))*?\)\$\]",
                      RegexOptions.IgnoreCase |
                      RegexOptions.Singleline);

        public string ReplaceTokens(IPage page, 
            StringBuilder webpartHeaders, 
            out bool havingWebPart, 
            ControllerContext controllerContext = null,
            bool isEditState = false)
        {
            var layout = _db.Set<PageLayout>().SingleOrDefault(e => e.Id == page.Layout);
            if (layout == null)
            {
                layout = _db.Set<PageLayout>().First();
            }
            string content;
            try
            {
                Dictionary<string, string> replacements = JsonConvert.DeserializeObject<Dictionary<string, string>>(page.ContentHtml.Content);
                if(page.Layout == 10 && isEditState ==false)
                {
                    if (replacements.Count > 0)
                    {
                       replacements.Remove("Summaryregion");
                    }
                }
                content = JsonTemplateEngine.BuildPageContentFromTemplate(layout.Template, replacements);
            }
            catch (Exception)
            {
                content = page.ContentHtml.Content;
            }
            return ReplaceTokens(
                page: page,
                content: content,
                webpartHeaders: webpartHeaders,
                havingWebPart: out havingWebPart,
                controllerContext: controllerContext,
                isEditState: isEditState);
        }

        private string ReplaceTokens(IPage page, 
            string content,
            StringBuilder webpartHeaders, 
            out bool havingWebPart, 
            bool isEditState,
            ControllerContext controllerContext)
        {
            content = content == null ? "" : content;
            
            bool _havingWebPart = false;
            string result = functionRegex.Replace(content, delegate(Match m)
            {
                _havingWebPart = true;
                return Callback(m,
                    page,
                    webpartHeaders, 
                    isEditState,
                    controllerContext);
            });

            havingWebPart = _havingWebPart;

            return result;
        }

        private string Callback(Match match, 
            IPage page, StringBuilder webpartHeaders, 
            bool isEditState,
            ControllerContext controllerContext)
        {
            var functionName = match.Groups["functionName"].Value;
            var paramList = CreateParamList(match);

            switch (functionName.ToUpper())
            {
                case "WEBPART":
                    return FuncWebPart(page,
                        paramList, webpartHeaders,
                        isEditState, 
                        controllerContext);
            }

            return functionName + string.Join(",", paramList);
        }

        private string FuncWebPart(IPage page, 
            string[] paramList, 
            StringBuilder webpartHeaders, 
            bool isEditState,
            ControllerContext controllerContext)
        {
            var webPartName = paramList.Length > 0 ? paramList[0] : "";
                        
            var driver = _coordinator.Apply(new DriverContext
            {
                WebpartId = webPartName.ToUpper(),
                Page = page,
                Headers = webpartHeaders,
                ControllerContext = controllerContext,
                Parameters = paramList,
                IsPublic = IsPublic
            });

            var result = isEditState ? driver.BuildEditor() : driver.BuildDisplay();

            if(result == DriverResult.Empty)
            {
                return webPartName;
            }
            else
            {
                return result.Content;
            }
        }

        
        private string[] CreateParamList(Match m)
        {
            var paramList = new string[m.Groups[2].Captures.Count];
            for (int i = 0; i < paramList.Length; i++)
            {
                paramList[i] = m.Groups["params"].Captures[i].Value;
            }
            return paramList;
        }
    }
}
