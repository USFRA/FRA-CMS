using System;
namespace RadCms.Helpers
{
    public interface IDriverCoordinator
    {
        IWebpartDriver Apply(DriverContext context);
    }
}
