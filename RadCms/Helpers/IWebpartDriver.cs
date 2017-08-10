using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadCms.Helpers
{
    public interface IWebpartDriver
    {
        string WebpartId { get;}
        void Apply(DriverContext context);
        DriverResult BuildDisplay();
        DriverResult BuildEditor();
    }
}
