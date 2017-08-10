using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RadCms.Entities;
using RadCms.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RadCms.Tests.Abstract
{
    [TestClass]
    public abstract class WebpartDriverTests
    {
        protected abstract IWebpartDriver CreateInstance();

        private DriverContext _cmsDriverContext;
        private DriverContext _pubDriverContext;
        private IWebpartDriver _driver;

        [TestInitialize]
        public void SetupDriverTest()
        {
            _driver = CreateInstance();
            var pageHtml = new CmsPageHtml()
            {
                Id = 1,
                Content = "[$webpart(" + _driver.WebpartId + ")$]",
                Header = "",
                Sidebar = "",
                Summary = ""
            };
            var contentType = new ContentType()
            {
                Id = 1,
                Title = "Page"
            };
            var pageLayout = new PageLayout()
            {
                Id = 1,
                Image = new byte[10],
                IsVisible = true,
                Order = 1,
                Style = "",
                Template = "",
                Title = "Mock Layout",
                Type = contentType
            };
            var naviHeading = new NaviHeading()
            {
                Id = 1,
                Description = "Mock NaviHeading",
                HeadingOrder = 1,
                Url = "MockHeading"
            };
            var naviNode = new NaviNode()
            {
                Id = 1,
                CreatedBy = "tianqu.liu.ctr",
                Created = DateTime.Now,
                Breadcrumb = "Mock Page",
                DefaultPageId = 1,
                Hidden = false,
                IsSecure = false,
                MenuOrder = 1,
                Modified = DateTime.Now,
                ModifiedBy = "tianqu.liu.ctr",
                NaviHeadings = new List<NaviHeading>() {
                    naviHeading
                },
                NodeName = "Mock Node",
                Parent = null,
                Pages = new List<CmsPage>(),
                SubNodes = new List<NaviNode>(),
                Type = contentType
            };
            var cmsPage = new CmsPage()
            {
                Id = 1,
                Commentable = false,
                Created = DateTime.Now,
                CreatedBy = "tianqu.liu.ctr",
                Modified = DateTime.Now,
                ModifiedBy = "tianqu.liu.ctr",
                Description = "Mock page",
                FriendlyUrl = "Mock/Page",
                Hidden = false,
                Html = pageHtml,
                IsPublished = true,
                Keywords = "",
                Layout = pageLayout.Id,
                Title = "Mock CMS Page",
                Type = contentType,
                MenuOrder = 1,
                NaviNode = naviNode,
                NaviTitle = "Mock Title",
                Url = "MockCmsPage"
            };

            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("GET");
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request).Returns(request.Object);
            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            _cmsDriverContext = new DriverContext
            {
                ControllerContext = controllerContext,
                Headers = new StringBuilder(),
                IsPublic = false,
                Page = cmsPage,
                Parameters = { }
            };

            _pubDriverContext = new DriverContext
            {
                ControllerContext = controllerContext,
                Headers = new StringBuilder(),
                Page = cmsPage,
                IsPublic = true,
                Parameters = { }
            };
        }

        [TestMethod()]
        public void ApplyTest()
        {
            _driver.Apply(_cmsDriverContext);
            _driver.Apply(_pubDriverContext);
        }

        [TestMethod()]
        public void BuildDisplayTest()
        {
            _driver.Apply(_cmsDriverContext);
            var driverResult = _driver.BuildDisplay();
            Assert.AreNotEqual(driverResult, DriverResult.Empty);
            Assert.AreEqual(driverResult.Content.Contains("data-replace=\"[$webpart(" + _driver.WebpartId + ")$]\""), true);
        }

        [TestMethod()]
        public void BuildEditorTest()
        {
            _driver.Apply(_cmsDriverContext);
            var driverResult = _driver.BuildEditor();
            Assert.AreNotEqual(driverResult, DriverResult.Empty);
            Assert.AreEqual(driverResult.Content.Contains("data-replace=\"[$webpart(" + _driver.WebpartId + ")$]\""), true);
        }
    }
}