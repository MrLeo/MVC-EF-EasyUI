using LYZJ.UserLimitMVC.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LYZJ.UserLimitMVC.UI.Portal.Controllers
{
    /// <summary>
    /// 基类BaseController，过滤器
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 当前登录的用户属性
        /// </summary>
        public BaseUser CurrentUserInfo { get; set; }

        /// <summary>
        /// 重新基类在Action执行之前的事情
        /// </summary>
        /// <param name="filterContext">重写方法的参数</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //得到用户登录的信息
            CurrentUserInfo = Session["UserInfo"] as BaseUser;
            //判断用户是否为空
            if (CurrentUserInfo == null)
            {
                Response.Redirect("/Login/Index");
            }
        }

        /// <summary>
        /// 返回处理过的时间的Json字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ContentResult JsonDate(object date)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return Content(JsonConvert.SerializeObject(date, Formatting.Indented, timeConverter));
        }
    }
}
