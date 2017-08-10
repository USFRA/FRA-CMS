using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadCms.Helpers
{
    public class DriverResult
    {
        private static DriverResult _empty = new DriverResult();
        public static DriverResult Empty
        {
            get
            {
                return _empty;
            }
        }

        public string Content { get; set; }
    }
}
