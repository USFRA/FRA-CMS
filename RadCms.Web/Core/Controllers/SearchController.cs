using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RadCms.Models;
using System.Net;
using System.Xml.Linq;
using RadCms.Mvc;

namespace RadCms.Controllers
{
    public class SearchController : CmsControllerBase
    {
        public ActionResult Search(string q, string site, string sort, string start)
        {
            if (site == null)
            {
                q = q + "&output=xml&sort=date%3AD%3AL%3Ad1&entqr=0&entqrm=0&oe=UTF-8&ie=UTF-8&ud=1&filter=0&site=FRA_Pages";
            }
            else
                q += "&output=xml&sort=" + (sort != null ? sort : "") + "&entqr=0&entqrm=0&oe=UTF-8&ie=UTF-8&ud=1&site=" + (site != null ? site : "") + "&filter=0&start=" + (start != null ? start : "");

            // PARSE HTML

            //string urlString = "http://google2.dot.gov/search?q=" + query + "&client=default_frontend&output=xml_no_dtd&proxystylesheet=default_frontend&sort=date%3AD%3AL%3Ad1&entqr=0&entqrm=0&oe=UTF-8&ie=UTF-8&ud=1&site=FRA_Pages";
            //Uri searchUrl = new Uri(urlString); 
            //string htmlString = new WebClient().DownloadString(searchUrl);
            //return View(htmlString);

            // PARSE XML

            string urlString = "http://google2.dot.gov/search?q=" + q;

            Uri searchUrl = new Uri(urlString);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string xmlString = client.DownloadString(searchUrl);
            XDocument doc = XDocument.Parse(xmlString);
            FraSearchModel model = new FraSearchModel();
            model.ResultList = new List<SearchResult>();
            List<SearchResult> searchResults = model.ResultList as List<SearchResult>;

            foreach (var p in doc.Root.Elements("PARAM"))
            {
                string value = p.Attribute("name") == null ? "" : p.Attribute("name").Value;
                switch (value)
                {
                    case ("sort"):
                        if (value.StartsWith("relevance"))
                            model.Sort = "relevance";
                        else
                            model.Sort = "date";
                        break;
                    case ("site"):
                        model.Site = p.Attribute("value") == null ? "" : p.Attribute("value").Value;
                        break;
                    case ("start"):
                        model.Start = Convert.ToInt32(p.Attribute("value") == null ? "" : p.Attribute("value").Value);
                        break;
                    default:
                        break;
                }
            }
            if (doc.Descendants("Suggestion") == null)
            {
                model.Suggestion = "";
            }
            else
            {
                foreach (var result in doc.Descendants("Suggestion"))
                {
                    model.Suggestion = result.Attribute("q") == null ? "" : result.Attribute("q").Value;
                }
            }
            model.Time = doc.Root.Element("TM").Value;
            model.Query = doc.Root.Element("Q") == null ? "" : doc.Root.Element("Q").Value;
            model.Total = (doc.Root.Element("RES") == null || doc.Root.Element("RES").Element("M") == null) ? 0 : Convert.ToInt32(doc.Root.Element("RES").Element("M").Value);
            model.PreviousLink = (doc.Root.Element("RES") == null || doc.Root.Element("RES").Element("NB") == null || doc.Root.Element("RES").Element("NB").Element("PU") == null) ? "#" : "/search" + doc.Root.Element("RES").Element("NB").Element("PU").Value;
            model.NextLink = (doc.Root.Element("RES") == null || doc.Root.Element("RES").Element("NB") == null || doc.Root.Element("RES").Element("NB").Element("NU") == null) ? "#" : "/search" + doc.Root.Element("RES").Element("NB").Element("NU").Value;
            foreach (var result in doc.Descendants("R"))
            {
                searchResults.Add(new SearchResult
                {
                    Url = AttrToString(result.Element("U")),
                    Title = AttrToString(result.Element("T")),
                    Subject = AttrToString(result.Element("S")).Replace("<br>", " "),
                    MIMEType = FormatMIMEType(AttrToString(result.Attribute("MIME"))),
                    Date = (result.Element("FS") == null || result.Element("FS").Attribute("VALUE") == null) ? "" : result.Element("FS").Attribute("VALUE").Value,
                    NodeNumber = result.Attribute("N") == null ? -1 : Convert.ToInt32(result.Attribute("N").Value),
                    LeafNumber = result.Attribute("L") == null ? -1 : Convert.ToInt32(result.Attribute("L").Value)
                });
            }

            return View(model);
        }

        /*public ActionResult Search(string q, string site, string sort, string start)
        {
            int pageSize = 10;// fixed for the webservice
            string query = q;
            if (site == null)
            {
                query += "&sort_by=date&affiliate=fra";
            }
            else
            {
                int page = 1;
                try
                {
                    var tempStart = Int32.Parse(start);
                    page = 1 + (tempStart - 1) / 10;
                }
                catch (Exception e)
                { 
                }
                
                query += "&sort_by=" + (sort != null ? sort : "date") + "&page=" + page + "&affiliate=" + (site != null ? site : "fra");
            }

            // PARSE XML
            string urlString = "http://search.usa.gov/api/search.xml?hl=true&query=" + query;

            Uri searchUrl = new Uri(urlString);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string xmlString = client.DownloadString(searchUrl);
            XDocument doc = XDocument.Parse(xmlString);
            FraSearchModel model = new FraSearchModel();
            model.ResultList = new List<SearchResult>();
            List<SearchResult> searchResults = model.ResultList as List<SearchResult>;

            model.Sort = string.IsNullOrEmpty(sort)?"date":sort;
            model.Site = string.IsNullOrEmpty(site)?"fra":site;
            model.Start = (doc.Root.Element("startrecord") == null) ? 0 : Convert.ToInt32(doc.Root.Element("startrecord").Value) - 1;
            model.Query = q;

            model.Total = (doc.Root.Element("total") == null) ? 0 : Convert.ToInt32(doc.Root.Element("total").Value);
            model.PreviousLink = model.Start == 0 ? "#" : "/search?q=" + q + "&site=fra&sort=" + model.Sort + "&start=" + (model.Start - pageSize + 1);
            model.NextLink = model.Start + pageSize + 1 > model.Total  ? "#" : "/search?q=" + q + "&site=fra&sort=" + model.Sort + "&start=" + (model.Start + pageSize + 1);
            foreach (var result in doc.Descendants("result"))
            {
                searchResults.Add(new SearchResult
                {
                    Url = AttrToString(result.Element("unescapedUrl")),
                    Title = AttrToString(result.Element("title")).Replace("", "<b>").Replace("", "</b>"),
                    Subject = AttrToString(result.Element("content")).Replace("", "<b>").Replace("", "</b>"),//.Replace("<br>", " "),
                    //MIMEType = FormatMIMEType(AttrToString(result.Attribute("MIME"))),
                    //Date = (result.Element("FS") == null || result.Element("FS").Attribute("VALUE") == null) ? "" : result.Element("FS").Attribute("VALUE").Value,
                    //NodeNumber = result.Attribute("N") == null ? -1 : Convert.ToInt32(result.Attribute("N").Value),
                    //LeafNumber = result.Attribute("L") == null ? -1 : Convert.ToInt32(result.Attribute("L").Value)
                });
            }

            return View(model);
        }*/

        private string AttrToString(XAttribute attr)
        {
            return attr == null ? "" : attr.Value;
        }

        private string AttrToString(XElement attr)
        {
            return attr == null ? "" : attr.Value;
        }

        private string FormatMIMEType(string attr)
        {
            switch (attr)
            {
                case "":
                case "text/plain":
                    return "PAGE";
                case "application/pdf":
                    return "PDF";
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                case "application/msword":
                    return "WORD";
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                case "application/vnd.ms-excel":
                    return "EXCEL";
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                case "application/vnd.ms-powerpoint":
                    return "POWERPOINT";
                default:
                    return attr.ToUpper();
            }
        }

        //
        // GET: /Search/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Search/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Search/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Search/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Search/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Search/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Search/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Search/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
