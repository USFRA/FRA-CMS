using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteTester.Assertions;

namespace RadCms.Core.Test.Routes
{
    public class VSUnitAssertEngine : IAssertEngine
    {
        public void Fail(string message)
        {
            Assert.Fail(message);
        }

        public void StringsEqualIgnoringCase(string s1, string s2, string message)
        {
            if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
            {
                return;
            }

            Assert.AreEqual(s1, s2, true, message);
        }
    }
}
