using System.Web.Mvc;
using System.Web.Routing;

namespace Epam_FinalProject_FileManager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ShareFileAccess",
                url: "F/S/{shareLink}",
                defaults: new
                {
                    controller = "MyStorage",
                    action = "SharedFile",
                    shareLink = UrlParameter.Optional
                });

            routes.MapRoute(
    name: "MyStorageCompression",
    url: "MyStorage/SetCompression/{selectedCompression}",
    defaults: new
    {
        controller = "MyStorage",
        action = "SetCompression",
        selectedCompression = UrlParameter.Optional
    });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
