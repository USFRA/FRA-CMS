using System;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using System.Web;

namespace RadCms.Mvc.ViewEngines.Razor
{
    /// <summary>
    /// Encapsulates a way to create controller and view using current active HTTP context.
    /// </summary>
    public static class ViewRenderer
    {
        /// <summary>
        /// Initializes a new instance of the generic controller class
        /// by using current active HTTP context, URL route data.
        /// </summary>
        /// <typeparam name="T">The type of controller class.</typeparam>
        /// <param name="routeData">The route data.</param>
        /// <returns>The controller.</returns>
        public static T CreateController<T>(RouteData routeData = null)
    where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper;
            if (HttpContext.Current != null)
                wrapper = new HttpContextWrapper(HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no " +
                    "active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") &&
                !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller",
                                     controller.GetType()
                                               .Name.ToLower().Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

        /// <summary>
        /// Renders a view to HTML string
        /// by using the specific Controller context, the Razor view path, the view model.
        /// </summary>
        /// <param name="context">The Controller context</param>
        /// <param name="viewPath">The path of the view that is rendered.</param>
        /// <param name="model">The model that is rendered by the view.</param>
        /// <param name="partial">Indicates whether the provided view is a partial view.</param>
        /// <returns>The HTML string of the view rendered.</returns>
        public static string RenderViewToString(ControllerContext context,
                                    string viewPath,
                                    object model = null,
                                    bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = System.Web.Mvc.ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
    }

    /// <summary>
    /// A mock controller.
    /// </summary>
    public class GenericController : Controller
    {
    }
}
