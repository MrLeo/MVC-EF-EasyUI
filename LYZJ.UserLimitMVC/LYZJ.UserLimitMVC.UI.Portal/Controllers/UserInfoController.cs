using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LYZJ.UserLimitMVC.BLL;
using LYZJ.UserLimitMVC.Model;
using LYZJ.UserLimitMVC.Common.Enum;
using LYZJ.UserLimitMVC.Common;
using LYZJ.UserLimitMVC.Common.select;

namespace LYZJ.UserLimitMVC.UI.Portal.Controllers
{
    public class UserInfoController : Controller
    {
        /// <summary>
        /// 在这里也需要依赖接口编程，用户接口
        /// </summary>
        private readonly IBLL.IBaseUserService _userInfoService = new BaseUserService();

        /// <summary>
        /// 依赖接口编程，角色接口信息
        /// </summary>
        private readonly IBLL.IBaseRoleService _roleInfoService = new BaseRoleService();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取所有的用户信息
        /// </summary>
        /// <returns>返回用户详细信息的Json对象</returns>
        public ActionResult GetAllUserInfos()
        {
            //Json格式的要求{total:22,rows:{}}

            //实现对用户分页的查询，rows：一共多少条，page：请求的当前第几页
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            //得到多条件查询的参数
            string RealName = Request["RealName"];
            string Telephone = Request["Telephone"];
            string EMail = Request["EMail"];
            int? Enabled = Request["Enabled"] == null ? -1 : int.Parse(Request["Enabled"]);
            string AuditStatus = Request["AuditStatus"];
            int? DeletionStateCode = Request["DeletionStateCode"] == null ? 0 : int.Parse(Request["DeletionStateCode"]);
            int total = 0;

            //调用分页的方法，传递参数,拿到分页之后的数据
            //var data = _userInfoService.LoadPageEntities(pageIndex, pageSize, out total, 
            //    u => true && u.DeletionStateCode == 0, true, u => u.SortCode);

            //封装一个业务逻辑层的方法，来处理分页过滤事件
            var userInfoQuery = new UserInfoQuery()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                RealName = RealName,
                Telephone = Telephone,
                EMail = EMail,
                Enabled = Enabled,
                AuditStatus = AuditStatus,
                Total = 0,
                DeletionStateCod = DeletionStateCode
            };
             //如果含有导航属性关联的话，出现循环引用的问题，死循环
            var data = from u in _userInfoService.LoadSearchData(userInfoQuery)
                       select new
                       {
                           u.ID,u.AuditStatus,u.Birthday,u.ChangePasswordDate,u.Code,u.CreateBy,u.CreateOn,u.CreateUserID,
                           u.DeletionStateCode,u.DepartmentID,u.Description,u.Email,u.Enabled,u.Gender,u.HomeAddress,u.IsStaff,
                           u.IsVisible,u.Mobile,u.ModifiedBy,u.ModifiedUserID,u.ModifirdOn,u.QICQ,u.QuickQuery,u.RealName,
                           u.SecurityLevel,u.SortCode,u.Telephone,u.Title,u.UserFrom,u.UserName,u.UserPassword
                       };
            //构造成Json的格式传递
            var result = new { total = userInfoQuery.Total, rows = data };
            //return JsonDate(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo">实体类(用户)</param>
        /// <returns>用户是否添加成功的标志</returns>
        public ActionResult RegisterUser(BaseUser userInfo)
        {
            //首先保存一些需要录入数据库的信息
            userInfo.Code = Guid.NewGuid().ToString();  //随机产生的一些数据
            userInfo.QuickQuery = userInfo.UserName;   //获取数据的查询码
            userInfo.UserFrom = "添加";               //用户来源
            userInfo.Lang = "汉语";                   //默认系统识别的是汉语
            userInfo.IsStaff = (Int32?)StaffEnum.OK;         //默认是职员
            userInfo.IsVisible = (Int32?)VisibleEnum.OK;     //默认显示信息
            userInfo.Enabled = (Int32?)EnabledEnum.OK;       //默认用户有效
            userInfo.AuditStatus = "已审核";         //默认添加的用户已经经过审核
            userInfo.DeletionStateCode = (Int32?)DeletionStateCodeEnum.Normal;    //默认没有伪删除
            userInfo.CreateOn = DateTime.Parse(DateTime.Now.ToString());     //默认创建用户日期
            BaseUser user = Session["UserInfo"] as BaseUser;
            userInfo.CreateUserID = user.Code;   //获取添加此用户的管理者的ID
            userInfo.CreateBy = user.UserName;//获取添加此用户的管理者的名称
            //执行添加用户的代码
            _userInfoService.AddEntity(userInfo);
            return Content("OK");
        }

        /// <summary>
        /// 验证用户名不能重复
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回用户名是否重复的标志</returns>
        public ActionResult CheckUserName(string UserName)
        {
            var checkUserName = _userInfoService.CheckUserNameTest(UserName);
            if (checkUserName == LoginResult.OK)
            {
                return Content("error");
            }
            return Content("OK");
        }

        /// <summary>
        /// 直接删除用户的信息
        /// </summary>
        /// <param name="userInfo">实体类</param>
        /// <param name="ID">主键ID</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Not">标志是否伪删除还有还原</param>
        /// <returns>返回执行成功的标志</returns>
        public ActionResult DeleteUsers(BaseUser userInfo, string ID, string UserName, string Not)
        {
            //首先判断是那个用户登录进入的，如果此用户正在使用这个系统，则不允许用户删除
            userInfo = Session["UserInfo"] as BaseUser;
            var userName = userInfo.UserName; //登录用户的信息
            var uIDsName = UserName.Split(',');  //将传递过来的用户名分割成一个一个的显示
            List<string> listUserInfo = new List<string>();
            foreach (var Name in uIDsName)
            {
                listUserInfo.Add(Name);
            }
            if (listUserInfo.Contains(userName))
            {
                return Content("含有正在使用的用户，禁止删除");
            }
            //下面我们开始删除用户的信息
            //首先判断确认是否从前台传递过来了信息
            if (string.IsNullOrEmpty(ID))
            {
                return Content("请选择需要删除的数据");
            }
            var idStrs = ID.Split(',');  //截取传递过来的字符串
            List<int> deleteIDList = new List<int>();
            foreach (var idStr in idStrs)
            {
                deleteIDList.Add(int.Parse(idStr));
            }
            if (Not == "not")
            {
                //伪删除,也就是根据用户的ID修改信息，首先查询出实体信息
                foreach (var deleteId in deleteIDList)
                {
                    var EditUserDeleteIsNot = _userInfoService.LoadEntities(c => c.ID == deleteId).FirstOrDefault();
                    EditUserDeleteIsNot.DeletionStateCode = 1;
                    _userInfoService.UpdateEntity(userInfo);
                }
                return Content("OK");
            }
            else if (Not == "back")
            {
                foreach (var deleteID in deleteIDList)
                {
                    var BackUserDelete = _userInfoService.LoadEntities(c => c.ID == deleteID).FirstOrDefault();
                    BackUserDelete.DeletionStateCode = 0;
                    _userInfoService.UpdateEntity(userInfo);
                }
                return Content("OK");
            }
            else
            {
                //最后执行批量删除数据的方法
                if (_userInfoService.DeleteUsers(deleteIDList) > 0)
                {
                    return Content("OK");
                }
            }
            return Content("删除失败，请您检查");
        }

        /// <summary>
        /// 根据用户ID读取当前用户的信息
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <returns>返回执行成功的信息</returns>
        public JsonResult GetUserInfos(int ID)
        {
            //根据ID的到当前选中的用户的信息
            //var userInfos = _userInfoService.LoadEntities(c => c.ID == ID).FirstOrDefault();
            var userInfos = (from u in _userInfoService.LoadEntities(c => c.ID == ID)
                             select new
                             {
                                 u.ID,u.AuditStatus,u.Birthday,u.CreateBy,u.CreateOn,u.CreateUserID,u.DeletionStateCode,
                                 u.Description,u.Email,u.Enabled,u.Gender,u.HomeAddress,u.IsStaff,u.IsVisible,u.Mobile,u.ModifiedBy,
                                 u.ModifiedUserID,u.ModifirdOn,u.QICQ,u.QuickQuery,u.RealName,u.SecurityLevel,u.SortCode,
                                 u.Telephone,u.Title,u.UserFrom,u.UserName,u.UserPassword
                             }).FirstOrDefault();
            return Json(userInfos, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据用户ID信息修改用户的信息
        /// </summary>
        /// <param name="userInfo">用户的实体类</param>
        /// <returns>返回是否修改成功的标志</returns>
        public ActionResult UpdateUserInfo(BaseUser userInfo)
        {
            //首先根据传递过来的参数查询出要修改的信息
            var editUserInfo = _userInfoService.LoadEntities(c => c.ID == userInfo.ID).FirstOrDefault();
            if (editUserInfo == null)
            {
                return Content("错误信息，请您检查");
            }
            //对用户的信息进行修改
            editUserInfo.UserName = userInfo.UserName;
            editUserInfo.RealName = userInfo.RealName;
            editUserInfo.QuickQuery = userInfo.UserName;
            editUserInfo.Email = userInfo.Email;
            editUserInfo.SecurityLevel = userInfo.SecurityLevel;
            editUserInfo.Gender = userInfo.Gender;
            editUserInfo.Birthday = userInfo.Birthday;
            editUserInfo.Mobile = userInfo.Mobile;
            editUserInfo.Telephone = userInfo.Telephone;
            editUserInfo.QICQ = userInfo.QICQ;
            editUserInfo.SortCode = userInfo.SortCode;
            editUserInfo.IsStaff = userInfo.IsStaff;
            editUserInfo.IsVisible = userInfo.IsVisible;
            editUserInfo.Enabled = userInfo.Enabled;
            editUserInfo.AuditStatus = userInfo.AuditStatus;
            editUserInfo.Description = userInfo.Description;
            editUserInfo.ModifirdOn = DateTime.Parse(DateTime.Now.ToString());
            BaseUser user = Session["UserInfo"] as BaseUser;
            editUserInfo.ModifiedUserID = user.Code;  //获取修改信息的ID
            editUserInfo.ModifiedBy = user.UserName;//获取修改此用户的用户名

            if (_userInfoService.UpdateEntity() > 0)
            {
                return Content("OK");
            }
            return Content("Error");
        }

        /// <summary>
        /// 为用户设置角色
        /// </summary>
        /// <param name="ID">获取当前选择的用户的ID</param>
        /// <returns>返回根据这个ID查到的用户信息</returns>
        public ActionResult SetRole(int ID)
        {
            var currentSetRoleUser = _userInfoService.LoadEntities(c => c.ID == ID).FirstOrDefault();
            //把当前要设置角色的用户传递到前台
            ViewData.Model = currentSetRoleUser;
            //前台需要所有的角色的信息，这时候我们就需要引用到所有的角色信息，便要定义角色类型
            //得到枚举中的没有被删除的信息
            const int deleteNorMal = (int) DeletionStateCodeEnum.Normal;
            var allRoles = _roleInfoService.LoadEntities(c => c.DeletionStateCode == deleteNorMal).ToList();
            //动态的MVC特性，传递角色的全部信息
            ViewBag.AllRoles = allRoles;
            //往前台传递用户已经关联了的角色信息
            if (currentSetRoleUser != null)
                ViewBag.ExtIsRoleIDS = (from r in currentSetRoleUser.R_UserInfo_Role
                                        //当前用户和角色中间表的集合数据
                                        select r.RoleID).ToList();
            return View();
        }

        /// <summary>
        /// 给用户设置角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetRole()
        {
            //首先获取设置角色的用户ID,查询出用户的信息
            var userId = Request["HideUserID"] == null ? 0 : int.Parse(Request["HideUserID"]);
            var currentSetUser = _userInfoService.LoadEntities(c => c.ID == userId).FirstOrDefault();
            if (currentSetUser != null)
            {
                //给当前用户设置角色，从前台拿到所有的 角色 sru_3，从请求的表单里面拿到所有的以sru_开头的key。
                //第一种方法
                //foreach (var allKey in Request.Form.AllKeys)
                //{
                //}
                //第二种写法
                var allKeys = from key in Request.Form.AllKeys
                              where key.StartsWith("sru_")
                              select key;
                //首先顶一个list集合存放传递过来的key，也就是角色的ID
                var roleIDs = new List<int>();
                //循环将角色的ID加入到集合中
                if (userId > 0)
                {
                    foreach (var key in allKeys)
                    {
                        roleIDs.Add(int.Parse(key.Replace("sru_", "")));
                    }
                }
                _userInfoService.SetBaseUserRole(userId, roleIDs,Session["UserInfo"] as BaseUser);
            }

            return Content("OK");
        }
    }
}
