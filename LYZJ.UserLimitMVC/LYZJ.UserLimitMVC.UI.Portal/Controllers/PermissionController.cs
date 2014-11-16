using LYZJ.UserLimitMVC.Common.select;
using LYZJ.UserLimitMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LYZJ.UserLimitMVC.UI.Portal.Controllers
{
    public class PermissionController : BaseController
    {

        /// <summary>
        /// 实例化权限的接口信息，使之能够调用权限的信息
        /// </summary>
        IBLL.IBasePermissionService _permissionInfoService = new BLL.BasePermissionService();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 实现查询出所有的权限信息
        /// </summary>
        /// <returns>返回查询出来的权限信息的Json串</returns>
        public ActionResult GetAllPermissionInfos()
        {
            //首先得到传递过来的参数
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 10: int.Parse(Request["rows"]);
            int total = 0;
            //得到多条件查询的参数
            string PerMission = Request["perMission"];
            int? RequestHttpType = Request["requestHttpType"] == null ? -1 : int.Parse(Request["requestHttpType"]);
            int? ActionType = Request["ActionType"] == null ? -1 : int.Parse(Request["ActionType"]);
            int? Enabled = Request["enabled"] == null ? -1 : int.Parse(Request["enabled"]);
            int? DeletionStateCode = Request["DeletionStateCode"] == null ? 0 : int.Parse(Request["DeletionStateCode"]);
            //封装一个实体层表示查询的类
            var permissionInfo = new PermissionQuery()
                                                 {
                                                     PageIndex = pageIndex,
                                                     PageSize = pageSize,
                                                     Total = 0,
                                                     PerMission = PerMission,
                                                     ActionType = ActionType,
                                                     Enabled = Enabled,
                                                     RequestHttpType = RequestHttpType,
                                                     DeletionStateCode = DeletionStateCode
                                                 };

            var date = _permissionInfoService.LoadSearchDate(permissionInfo);
            var result = new {total = permissionInfo.Total, rows = date};
            return JsonDate(result);
        }

        /// <summary>
        /// 实现对权限的增加功能
        /// </summary>
        /// <param name="permission">权限的实体类</param>
        /// <returns>返回是否执行成功的权限的类型</returns>
        public ActionResult AddPermission(BasePermission permissionInfo)
        {
            permissionInfo.Code = Guid.NewGuid().ToString();
            //permissionInfo.ActionType
            permissionInfo.AllowEdit = 1;
            permissionInfo.AllowDelete = 1;
            permissionInfo.IsVisible = 1;
            permissionInfo.DeletionStateCode = 0;
            permissionInfo.Enabled = 1;
            permissionInfo.CreateOn = DateTime.Parse(DateTime.Now.ToString());
            BaseUser user = Session["UserInfo"] as BaseUser;
            permissionInfo.CreateUserID = user.Code;
            permissionInfo.CreateBy = user.UserName;
            //执行添加权限代码，返回OK
            _permissionInfoService.AddEntity(permissionInfo);
            return Content("OK");
        }

        /// <summary>
        /// 绑定显示用户的详细信息
        /// </summary>
        /// <param name="ID">查询的详细信息的ID</param>
        /// <returns>返回受影响的Json串</returns>
        public JsonResult GetPermissionInfos(int ID)
        {
            var temp = _permissionInfoService.LoadEntities(c => c.ID == ID).FirstOrDefault();
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 实现对权限的删除
        /// </summary>
        /// <param name="permissionInfo">权限的实体类</param>
        /// <returns>返回执行成功的标志</returns>
        public ActionResult UpdatePermissionInfo(BasePermission permissionInfo)
        {
            var editPermissionInfo = _permissionInfoService.LoadEntities(c => c.ID == permissionInfo.ID).FirstOrDefault();
            if (editPermissionInfo == null)
            {
                return Content("请您检查，错误信息");
            }
            editPermissionInfo.PerMission = permissionInfo.PerMission;
            editPermissionInfo.RequestHttpType = permissionInfo.RequestHttpType;
            editPermissionInfo.RequestURL = permissionInfo.RequestURL;
            editPermissionInfo.ActionType = permissionInfo.ActionType;
            editPermissionInfo.SortCode = permissionInfo.SortCode;
            editPermissionInfo.AllowDelete = permissionInfo.AllowDelete;
            editPermissionInfo.AllowEdit = permissionInfo.AllowEdit;
            editPermissionInfo.IsVisible = permissionInfo.IsVisible;
            editPermissionInfo.Enabled = permissionInfo.Enabled;
            editPermissionInfo.Description = permissionInfo.Description;
            editPermissionInfo.ModifiedOn = DateTime.Parse(DateTime.Now.ToString());
            BaseUser user = Session["UserInfo"] as BaseUser;
            editPermissionInfo.ModifiedUserID = user.Code;  //获取修改信息的ID
            editPermissionInfo.ModifiedBy = user.UserName;//获取修改此用户的用户名
            if (_permissionInfoService.UpdateEntity() > 0)
            {
                return Content("OK");
            }
            return Content("Error");
        }

        /// <summary>
        /// 实现删除权限的信息
        /// </summary>
        /// <param name="permissionInfo">权限的实体类</param>
        /// <param name="ID">权限的ID</param>
        /// <param name="Not">表示进行了什么操作</param>
        /// <returns>返回对权限操作的标识</returns>
        public ActionResult DeletePermission(BasePermission permissionInfo, string ID,string Not)
        {
            //判断是否传递过来了信息，如果没有传递过来信息，则提示错误
            if (string.IsNullOrEmpty(ID))
            {
                return Content("请您选择需要删除/还原的权限信息");
            }
            //解析传递过来的字符串
            var idStrs = ID.Split(',');
            List<int> list = new List<int>();
            foreach (var idStr in idStrs)
            {
                list.Add(int.Parse(idStr));
            }
            //伪删除角色信息
            if (Not == "not")
            {
                foreach (var permissionID in list)
                {
                    var deletePermissionInfo = _permissionInfoService.LoadEntities(c => c.ID == permissionID).FirstOrDefault();
                    deletePermissionInfo.DeletionStateCode = 1;
                    _permissionInfoService.UpdateEntity(permissionInfo);
                }
                return Content("OK");
            }
            //还原被伪删除掉的数据
            else if (Not == "back")
            {
                foreach (var permissionID in list)
                {
                    var backPermissionInfo = _permissionInfoService.LoadEntities(c => c.ID == permissionID).FirstOrDefault();
                    backPermissionInfo.DeletionStateCode = 0;
                    _permissionInfoService.UpdateEntity(permissionInfo);
                }
                return Content("OK");
            }
            //直接删除数据
            else
            {
                if (_permissionInfoService.DeletePermission(list) > 0)
                {
                    return Content("OK");
                }

            }
            return Content("Error");
        }
    }
}
