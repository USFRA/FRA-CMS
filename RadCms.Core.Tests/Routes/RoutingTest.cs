using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteTester;
using MvcRouteTester.HttpMocking;
using RadCms.Core.Containers;

namespace RadCms.Core.Test.Routes
{
    [TestClass]
    public class RoutingTest
    {
        private RouteCollection routes = new RouteCollection();
        [TestInitialize]
        public void SetupRouteTests()
        {
            RouteAssert.UseAssertEngine(new VSUnitAssertEngine());

            // Register Core Area
            var containersAreaContext = new AreaRegistrationContext("Containers", routes);
            var coreAreaReg = new ContainersAreaRegistration();
            coreAreaReg.RegisterArea(containersAreaContext);

            RouteConfig.RegisterRoutes(routes);
        }

        [TestMethod]
        public void PageControllerRouteTest()
        {
            var httpContext = HttpMockery.ContextForUrl("~/");
            var routeData = routes.GetRouteData(httpContext);

            Assert.IsNotNull(routeData, "Did not find route");
            Assert.AreEqual("cms", (string)routeData.Values["Controller"], true);
            Assert.AreEqual("page", (string)routeData.Values["Action"], true);
            Assert.AreEqual("Containers", (string)routeData.DataTokens["Area"], true);
        }
    }
}
