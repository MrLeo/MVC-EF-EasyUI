using LYZJ.UserLimitMVC.Common.select;
using LYZJ.UserLimitMVC.IBLL;
using LYZJ.UserLimitMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.BLL
{
    public partial class BasePermissionGroupService : BaseService<BasePermissionGroup>, IBasePermissionGroupService
    {
        /// <summary>
        /// 实现对菜单组的查询
        /// </summary>
        /// <param name="permission">传递的是菜单组查询的实体类</param>
        /// <returns>返回结果</returns>
        public IEnumerable<BasePermissionGroup> LoadSearchDate(PermissionGroupQuery permissionGroup)
        {
            var temp = _DbSession.BasePermissionGroupRepository.LoadEntities(c => true);
            if (!string.IsNullOrEmpty(permissionGroup.GroupName))
            {
                temp = temp.Where<BasePermissionGroup>(c => c.GroupName.Contains(permissionGroup.GroupName));
            }
            if (permissionGroup.GroupType != -1)
            {
                temp = temp.Where<BasePermissionGroup>(c => c.GroupType == permissionGroup.GroupType);
            }
            if (permissionGroup.DeletionStateCode == 1)
            {
                temp = temp.Where<BasePermissionGroup>(c => c.DeletionStateCode == permissionGroup.DeletionStateCode);
            }
            else
            {
                temp = temp.Where<BasePermissionGroup>(c => c.DeletionStateCode == 0);
            }
            permissionGroup.Total = temp.Count();
            return temp.OrderBy(c => c.SortCode)
                .Skip(permissionGroup.PageSize * (permissionGroup.PageIndex - 1))
                .Take(permissionGroup.PageSize);
        }

        /// <summary>
        /// 实现删除菜单组的操作
        /// </summary>
        /// <param name="list">菜单组的集合信息</param>
        /// <returns>返回受影响的行数</returns>
        public int DeletePermissionEntity(List<int> list)
        {
            foreach (var ID in list)
            {
                _DbSession.BasePermissionGroupRepository.DeleteEntity(new BasePermissionGroup()
                {
                    ID = ID
                });
            }
            return _DbSession.SaveChanges();
        }
    }
}
