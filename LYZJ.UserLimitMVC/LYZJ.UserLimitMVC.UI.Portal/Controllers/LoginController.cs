using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LYZJ.UserLimitMVC.BLL;
using LYZJ.UserLimitMVC.Common;
using LYZJ.UserLimitMVC.Model;

namespace LYZJ.UserLimitMVC.UI.Portal.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 实例化UserInfo接口的对象
        /// </summary>
        private IBLL.IBaseUserService _userInfoService = new BaseUserService();
        
        /// <summary>
        /// 实现用户的登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// 判断用户输入的信息是否正确，[HttpPost]
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="userInfo">用户的实体类</param>
        /// <param name="Code">验证码</param>
        /// <returns>返回是否执行成功的标志</returns>
        public ActionResult CheckUserInfo(string UserName, BaseUser userInfo, string Code)
        {
            //如果用户信息存在的话讲用户信息保存到session中
            if (UserName != null)
            {
                //首先根据用户名的信息获取到用户详细的信息
                BaseUser userInfoShow = _userInfoService.LoadEntities(c => c.UserName == UserName).FirstOrDefault();
                Session["UserInfo"] = userInfoShow;
            }

            //首先我们拿到系统的验证码
            string sessionCode = this.TempData["ValidateCode"] == null
                                     ? new Guid().ToString()
                                     : this.TempData["ValidateCode"].ToString();
            //然后我们就将验证码去掉，避免了暴力破解
            this.TempData["ValidateCode"] = new Guid();
            //判断用户输入的验证码是否正确
            if (sessionCode != Code)
            {
                return Content("验证码输入不正确");
            }

            //调用业务逻辑层（BLL）去校验用户是否正确,,,定义变量存取获取到的用户的错误信息
            string UserInfoError = "";
            var loginUserInfo = _userInfoService.CheckUserInfo(userInfo);
            switch (loginUserInfo)
            {
                case LoginResult.PwdError:
                    UserInfoError = "密码输入错误";
                    break;
                case LoginResult.UserNotExist:
                    UserInfoError = "用户名输入错误或者您已经被禁用";
                    break;
                case LoginResult.UserIsNull:
                    UserInfoError = "用户名不能为空";
                    break;
                case LoginResult.PwdIsNUll:
                    UserInfoError = "密码不能为空";
                    break;
                case LoginResult.OK:
                    UserInfoError = "OK";
                    break;
                default:
                    UserInfoError = "未知错误，请您检查您的数据库";
                    break;
            }

            #region ----使用if else来判断信息----
            //if (loginUserInfo == LoginResult.UserIsNull)
            //{
            //    UserInfoError = "用户名不能为空";
            //}
            //else if (loginUserInfo == LoginResult.PwdIsNUll)
            //{
            //    UserInfoError = "密码不能为空";
            //}
            //else if (loginUserInfo == LoginResult.UserNotExist)
            //{
            //    UserInfoError = "用户名输入错误";
            //}
            //else if (loginUserInfo == LoginResult.PwdError)
            //{
            //    UserInfoError = "密码输入错误";
            //}
            //else if (loginUserInfo == LoginResult.OK)
            //{
            //    UserInfoError = "OK";
            //}
            //else
            //{
            //    UserInfoError = "未知错误，请您检查您的数据库";
            //} 
            #endregion

            return Content(UserInfoError);
        }


        /// <summary>
        /// 验证码的实现
        /// </summary>
        /// <returns>返回验证码</returns>
        public ActionResult CheckCode()
        {   
            //首先实例化验证码的类
            KenceryValidateCode validateCode = new KenceryValidateCode();
            //生成验证码指定的长度
            //string code = validateCode.CreateValidateCode(5);
            string code = "11111";
            //将验证码赋值给Session变量
            //Session["ValidateCode"] = code;
            this.TempData["ValidateCode"] = code;
            //创建验证码的图片
            byte[] bytes = validateCode.CreateValidateGraphic(code);
            //最后将验证码返回
            return File(bytes, @"image/jpeg");
        }
    }
}