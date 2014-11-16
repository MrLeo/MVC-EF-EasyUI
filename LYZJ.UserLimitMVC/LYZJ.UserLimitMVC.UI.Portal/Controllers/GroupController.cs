using LYZJ.UserLimitMVC.Common.select;
using LYZJ.UserLimitMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LYZJ.UserLimitMVC.UI.Portal.Controllers
{
    public class GroupController : BaseController
    {
        /// <summary>
        /// 基于结构的编程，这里也可以直接放到构造函数中实力话
        /// </summary>
        IBLL.IBasePermissionGroupService _permissionGroupService = new BLL.BasePermissionGroupService();


        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 实现查询出所有的菜单组信息
        /// </summary>
        /// <returns>返回查询出来的菜单组的Json串</returns>
        public ActionResult GetAllPermissionGroups()
        {
            //首先拿到传递过来的参数
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int total = 0;

            //得到多条件查询和回收站的参数
            string GroupName = Request["GroupName"];
            int? GroupType = Request["GroupType"] == null ? -1 : int.Parse(Request["GroupType"]);
            int? DeletionStateCode = Request["DeletionStateCode"] == null ? -1 : int.Parse(Request["DeletionStateCode"]);

            //封装一个实体层表示查询的类
            PermissionGroupQuery permissionGroup = new PermissionGroupQuery()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                GroupName = GroupName,
                GroupType = GroupType,
                DeletionStateCode = DeletionStateCode
            };
            //根据条件查询结果
            var data = _permissionGroupService.LoadSearchDate(permissionGroup);
            var result = new { total = permissionGroup.Total, rows = data };
            return JsonDate(result);
        }

        /// <summary>
        /// 实现对菜单组的增加功能
        /// </summary>
        /// <param name="permission">菜单组的实体类</param>
        /// <returns>返回是否执行成功的菜单的类型</returns>
        public ActionResult AddPermissionGroup(BasePermissionGroup permissionGroup)
        {
            permissionGroup.AllowEdit = 1;
            permissionGroup.AllowDelete = 1;
            permissionGroup.IsVisible = 1;
            permissionGroup.DeletionStateCode = 0;
            permissionGroup.Enabled = 1;
            permissionGroup.CreateOn = DateTime.Parse(DateTime.Now.ToString());
            BaseUser user = Session["UserInfo"] as BaseUser;
            permissionGroup.CreateUserID = user.Code;
            permissionGroup.CreateBy = user.UserName;
            //执行添加代码，返回OK
            _permissionGroupService.AddEntity(permissionGroup);
            return Content("OK");
        }

        /// <summary>
        /// 实现删除菜单组的数据
        /// </summary>
        /// <param name="permissionInfo">菜单组的实体类</param>
        /// <param name="ID">权限的ID</param>
        /// <param name="Not">表示进行了什么操作</param>
        /// <returns>返回对权限操作的标识</returns>
        public ActionResult DeletePermissionGroup(BasePermissionGroup permissionGroup, string ID, string Not)
        {
            //首先判断是否传递过来了ID，如果传递过来了继续往下走，否则提示错误
            if (string.IsNullOrEmpty(ID))
            {
                return Content("请您选择需要删除/还原的信息");
            }
            //List集合解析传递过来的ID
            var idStrs = ID.Split(',');
            List<int> list = new List<int>();
            foreach (var idStr in idStrs)
            {
                list.Add(int.Parse(idStr));
            }
            //循环伪删除菜单组的数据
            if (Not == "not")
            {
                foreach (var permissionGroupID in list)
                {
                    var deletePermissionGroupInfo = _permissionGroupService.LoadEntities(c => c.ID == permissionGroupID).FirstOrDefault();
                    deletePermissionGroupInfo.DeletionStateCode = 1;
                    _permissionGroupService.UpdateEntity(permissionGroup);
                }
                return Content("OK");
            }
            //循环还原菜单组的数据
            else if (Not == "back")
            {
                foreach (var permissionGroupID in list)
                {
                    var deletePermissionGroupInfo = _permissionGroupService.LoadEntities(c => c.ID == permissionGroupID).FirstOrDefault();
                    deletePermissionGroupInfo.DeletionStateCode = 0;
                    _permissionGroupService.UpdateEntity(permissionGroup);
                }
                return Content("OK");
            }
            //循环直接删除菜单组的数据
            else
            {
                if (_permissionGroupService.DeletePermissionEntity(list) > 0)
                {
                    return Content("OK");
                }
            }
            return Content("Error");
        }

        /// <summary>
        /// 绑定显示菜单项的详细信息
        /// </summary>
        /// <param name="ID">查询的详细信息的ID</param>
        /// <returns>返回受影响的Json串</returns>
        public ActionResult GetPermissionGroupInfo(int ID)
        {
            var temp = _permissionGroupService.LoadEntities(c => c.ID == ID).FirstOrDefault();
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 实现对菜单项的删除
        /// </summary>
        /// <param name="permissionInfo">菜单项的实体类</param>
        /// <returns>返回执行成功的标志</returns>
        public ActionResult UpdatePermissionGroup(BasePermissionGroup permissionGroup)
        {
            var editPermissionGroupInfo = _permissionGroupService.LoadEntities(c => c.ID == permissionGroup.ID).FirstOrDefault();
            if (editPermissionGroupInfo == null)
            {
                return Content("请您检查，错误信息");
            }
            editPermissionGroupInfo.GroupName = permissionGroup.GroupName;
            editPermissionGroupInfo.GroupType = permissionGroup.GroupType;
            editPermissionGroupInfo.SortCode = permissionGroup.SortCode;
            editPermissionGroupInfo.AllowDelete = permissionGroup.AllowDelete;
            editPermissionGroupInfo.AllowEdit = permissionGroup.AllowEdit;
            editPermissionGroupInfo.IsVisible = permissionGroup.IsVisible;
            editPermissionGroupInfo.Enabled = permissionGroup.Enabled;
            editPermissionGroupInfo.Description = permissionGroup.Description;
            editPermissionGroupInfo.ModifiedOn = DateTime.Parse(DateTime.Now.ToString());
            BaseUser user = Session["UserInfo"] as BaseUser;
            editPermissionGroupInfo.ModifiedUserID = user.Code;
            editPermissionGroupInfo.ModifiedBy = user.UserName;
            if (_permissionGroupService.UpdateEntity() > 0)
            {
                return Content("OK");
            }
            return Content("Error");
        }
    }
}