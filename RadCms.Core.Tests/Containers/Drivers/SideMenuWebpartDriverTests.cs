using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadCms.Core.Tests.Abstract;
using RadCms.Helpers;

namespace RadCms.Core.Containers.Drivers.Tests
{
    [TestClass]
    public class SideMenuWebpartDriverTests: WebpartDriverTests
    {
        public override IWebpartDriver CreateInstance()
        {
            IPageUrlHelper urlHelper = new IdBasedUrlHelper();
            return new SideMenuWebpartDriver(urlHelper);
        }

        [TestInitialize]
        public void TestInit()
        {
            base.SetupDriverTest();
        }
    }
}