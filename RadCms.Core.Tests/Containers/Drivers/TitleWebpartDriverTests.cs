using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadCms.Core.Tests.Abstract;
using RadCms.Helpers;

namespace RadCms.Core.Containers.Drivers.Tests
{
    [TestClass]
    public class TitleWebpartDriverTests: WebpartDriverTests
    {
        public override IWebpartDriver CreateInstance()
        {
            return new TitleWebpartDriver();
        }

        [TestInitialize]
        public void TestInit()
        {
            base.SetupDriverTest();
        }
    }
}