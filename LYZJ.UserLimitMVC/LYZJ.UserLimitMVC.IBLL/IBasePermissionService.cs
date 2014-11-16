using LYZJ.UserLimitMVC.Common.select;
using LYZJ.UserLimitMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.IBLL
{
    public partial interface IBasePermissionService : IBaseService<BasePermission>
    {
        /// <summary>
        /// 实现对权限的查询
        /// </summary>
        /// <param name="permission">传递的是权限查询的实体类</param>
        /// <returns>返回结果</returns>
        IQueryable<BasePermission> LoadSearchDate(PermissionQuery permissionInfo);

        /// <summary>
        /// 实现删除权限的操作
        /// </summary>
        /// <param name="list">权限的集合信息</param>
        /// <returns>返回受影响的行数</returns>
        int DeletePermission(List<int> list);
    }
}
