using System;
using System.Web;

namespace RadCms.Security
{
    public class CustomServerHeaderModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += new EventHandler(OnPreSendRequestHeaders);
        }

        #endregion

        public void OnPreSendRequestHeaders(Object source, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Set("Server", "Webserver"); 
        }
    }
}
