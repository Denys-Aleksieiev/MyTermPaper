using System.Web;
using System.Web.Mvc;

namespace Epam_FinalProject_FileManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
