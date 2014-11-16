using LYZJ.UserLimitMVC.BLL;
using LYZJ.UserLimitMVC.Common.select;
using LYZJ.UserLimitMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LYZJ.UserLimitMVC.UI.Portal.Controllers
{
    public class RoleController : BaseController
    {
        /// <summary>
        /// 依赖接口编程，定义接口的对象
        /// </summary>
        public IBLL.IBaseRoleService _roleInfoService = new BaseRoleService();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获得角色的信息显示在角色列表中
        /// </summary>
        /// <returns>返回角色信息的Json对象</returns>
        public ActionResult GetAllRoleInfos()
        {
            //实现对用户和多条件的分页的查询，rows表示一共多少条，page表示当前第几页
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);

            string RealName = Request["RealName"];
            int? Enabled = Request["Enabled"] == null ? -1 : int.Parse(Request["Enabled"]);
            int? CategoryCode = Request["CategoryCode"] == null ? -1 : int.Parse(Request["CategoryCode"]);
            int? DeletionStateCode = Request["DeletionStateCode"] == null ? 0 : int.Parse(Request["DeletionStateCode"]);
            int total = 0;
            //封装一个业务逻辑层的方法来处理多条件查询的信息
            var roleinfo = new RoleInfoQuery()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                RealName = RealName,
                Enabled = Enabled,
                CategoryCode = CategoryCode,
                DeletionStateCode = DeletionStateCode,
                Total = 0
            };

            var date = from n in _roleInfoService.loadSearchDate(roleinfo)
                       select new
                       {
                           n.AllowDelete,n.AllowEdit,n.CategoryCode,n.Code,n.CreateBy,n.CreateOn,n.CreateUserID,n.DeletionStateCode,
                           n.Description,n.Enabled,n.ID,n.IsVisible,n.ModifiedBy,n.ModifiedOn,n.ModifiedUserID,n.Realname,
                           n.SortCode
                       };
            //构造Json对象返回
            var result = new { total = roleinfo.Total, rows = date };
            return JsonDate(result);
        }

        /// <summary>
        /// 实现对角色的添加
        /// </summary>
        /// <param name="roleInfo">角色的实体类</param>
        /// <returns>返回添加成功的标志</returns>
        public ActionResult AddRole(BaseRole roleInfo)
        {
            roleInfo.Code = Guid.NewGuid().ToString();
            roleInfo.CategoryCode = roleInfo.CategoryCode;
            roleInfo.Realname = roleInfo.Realname;
            roleInfo.AllowDelete = 1;
            roleInfo.AllowEdit = 1;
            roleInfo.IsVisible = 1;
            roleInfo.SortCode = roleInfo.SortCode;
            roleInfo.DeletionStateCode = 0;
            roleInfo.Enabled = 1;
            roleInfo.Description = roleInfo.Description;
            BaseUser user = Session["UserInfo"] as BaseUser;
            roleInfo.CreateUserID = user.Code;
            roleInfo.CreateBy = user.UserName;
            roleInfo.CreateOn = DateTime.Parse(DateTime.Now.ToString());

            //执行添加角色的代码，返回OK
            _roleInfoService.AddEntity(roleInfo);
            return Content("OK");
        }

        /// <summary>
        /// 实现对角色信息的批量删除和伪删除
        /// </summary>
        /// <param name="roleInfo">角色信息的实例</param>
        /// <param name="ID">选择的角色信息的ID</param>
        /// <param name="Not">标志是伪删除还是直接删除，还原</param>
        /// <returns>返回是否删除成功的标志</returns>
        public ActionResult DeleteRole(BaseRole roleInfo, string ID, string Not)
        {
            //首先判断是否从前台传递过来了需要删除的信息
            if (string.IsNullOrEmpty(ID))
            {
                return Content("请您选择需要删除/还原的角色信息");
            }
            //截取传递过来的字符串信息，来实现批量删除，还原的功能
            var idStrs = ID.Split(',');
            List<int> list = new List<int>();
            foreach (var idStr in idStrs)
            {
                list.Add(int.Parse(idStr));
            }
            //伪删除角色信息
            if (Not == "not")
            {
                foreach (var roleID in list)
                {
                    var deleteNotRole = _roleInfoService.LoadEntities(c => c.ID == roleID).FirstOrDefault();
                    deleteNotRole.DeletionStateCode = 1;
                    _roleInfoService.UpdateEntity(roleInfo);
                }
                return Content("OK");
            }
            //还原被伪删除掉的数据
            else if (Not == "back")
            {
                foreach (var roleID in list)
                {
                    var deleteNotRole = _roleInfoService.LoadEntities(c => c.ID == roleID).FirstOrDefault();
                    deleteNotRole.DeletionStateCode = 0;
                    _roleInfoService.UpdateEntity(roleInfo);
                }
                return Content("OK");
            }
            //最后执行直接删除
            else
            {
                if (_roleInfoService.DeleteRoles(list) > 0)
                {
                    return Content("OK");
                }
            }
            return Content("删除失败，请您检查");
        }

        /// <summary>
        /// 根据ID读取当前角色的信息
        /// </summary>
        /// <param name="ID">角色表的ID</param>
        /// <returns>返回查询到的角色表的信息</returns>
        public JsonResult GetRoleInfos(int ID)
        {
            var roleInfo = (from n in _roleInfoService.LoadEntities(c => c.ID == ID)
                            select new
                            {
                                n.ID,n.AllowDelete,n.AllowEdit,n.CategoryCode,n.CreateBy,n.CreateOn,n.CreateUserID,
                                n.DeletionStateCode,n.Description,n.Enabled,n.IsVisible,n.ModifiedBy,n.ModifiedOn,
                                n.ModifiedUserID,n.Realname,n.SortCode
                            }).FirstOrDefault();
            return Json(roleInfo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 实现对角色的修改
        /// </summary>
        /// <param name="roleInfo">传递过来角色的实体类</param>
        /// <returns>返回执行成功的标志</returns>
        public ActionResult UpdateRoleInfo(BaseRole roleInfo)
        {
            var editRoleInfo = _roleInfoService.LoadEntities(c => c.ID == roleInfo.ID).FirstOrDefault();
            if (editRoleInfo == null)
            {
                return Content("错误信息，请您检查");
            }
            editRoleInfo.Realname = roleInfo.Realname;
            editRoleInfo.CategoryCode = roleInfo.CategoryCode;
            editRoleInfo.AllowEdit = roleInfo.AllowEdit;
            editRoleInfo.IsVisible = roleInfo.IsVisible;
            editRoleInfo.SortCode = roleInfo.SortCode;
            editRoleInfo.Enabled = roleInfo.Enabled;
            editRoleInfo.Description = roleInfo.Description;
            editRoleInfo.ModifiedOn = DateTime.Parse(DateTime.Now.ToString());
            BaseUser user = Session["UserInfo"] as BaseUser;
            editRoleInfo.ModifiedUserID = user.Code;  //获取修改信息的ID
            editRoleInfo.ModifiedBy = user.UserName;//获取修改此用户的用户名
            if (_roleInfoService.UpdateEntity() > 0)
            {
                return Content("OK");
            }
            return Content("error");
        }

    }
}