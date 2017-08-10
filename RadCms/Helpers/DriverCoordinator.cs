using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadCms.Helpers
{
    public class DriverCoordinator : IDriverCoordinator
    {
        private IEnumerable<IWebpartDriver> _drivers;

        public DriverCoordinator(IEnumerable<IWebpartDriver> drivers)
        {
            _drivers = drivers;
        }

        public IWebpartDriver Apply(DriverContext context)
        {
            var driver = _drivers.SingleOrDefault(e => e.WebpartId.Equals(context.WebpartId, StringComparison.CurrentCultureIgnoreCase)) 
                ?? new DefaultWebpartDriver();
            driver.Apply(context);
            return driver;
        }
    }
}
