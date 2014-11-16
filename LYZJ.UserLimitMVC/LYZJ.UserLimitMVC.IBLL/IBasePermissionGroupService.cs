using LYZJ.UserLimitMVC.Common.select;
using LYZJ.UserLimitMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.IBLL
{
    public partial interface IBasePermissionGroupService : IBaseService<BasePermissionGroup>
    {
        /// <summary>
        /// 实现对菜单组的查询
        /// </summary>
        /// <param name="permission">传递的是菜单组查询的实体类</param>
        /// <returns>返回结果</returns>
        IEnumerable<BasePermissionGroup> LoadSearchDate(PermissionGroupQuery permissionGroup);

        /// <summary>
        /// 实现删除菜单组的操作
        /// </summary>
        /// <param name="list">菜单组的集合信息</param>
        /// <returns>返回受影响的行数</returns>
        int DeletePermissionEntity(List<int> list);
    }
}