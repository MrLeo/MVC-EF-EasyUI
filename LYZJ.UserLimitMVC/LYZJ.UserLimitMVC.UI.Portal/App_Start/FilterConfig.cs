using System.Web;
using System.Web.Mvc;

namespace LYZJ.UserLimitMVC.UI.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}