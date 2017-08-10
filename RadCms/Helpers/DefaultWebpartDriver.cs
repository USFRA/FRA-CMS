using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadCms.Helpers
{
    public class DefaultWebpartDriver : IWebpartDriver
    {
        public string WebpartId
        {
            get
            {
                return "";
            }
        }

        public void Apply(DriverContext context)
        {
        }

        public DriverResult BuildDisplay()
        {
            return DriverResult.Empty;
        }

        public DriverResult BuildEditor()
        {
            return DriverResult.Empty;
        }
    }
}
