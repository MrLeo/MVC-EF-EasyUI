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
    public partial class BasePermissionService : BaseService<BasePermission>, IBasePermissionService
    {
        /// <summary>
        /// 实现对权限的查询
        /// </summary>
        /// <param name="permission">传递的是权限查询的实体类</param>
        /// <returns>返回结果</returns>
        public IQueryable<BasePermission> LoadSearchDate(PermissionQuery permissionInfo)
        {
            var temp = _DbSession.BasePermissionRepository.LoadEntities(c => true);
            if (!string.IsNullOrEmpty(permissionInfo.PerMission))
            {
                temp = temp.Where<BasePermission>(c => c.PerMission.Contains(permissionInfo.PerMission));
            }
            if (permissionInfo.RequestHttpType != -1)
            {
                temp = temp.Where<BasePermission>(c => c.RequestHttpType == permissionInfo.RequestHttpType);
            }
            if (permissionInfo.ActionType != -1)
            {
                temp = temp.Where<BasePermission>(c => c.ActionType == permissionInfo.ActionType);
            }
            if (permissionInfo.Enabled != -1)
            {
                temp = temp.Where<BasePermission>(c => c.Enabled == permissionInfo.Enabled);
            }
            if (permissionInfo.DeletionStateCode == 1)
            {
                temp = temp.Where<BasePermission>(c => c.DeletionStateCode == permissionInfo.DeletionStateCode);
            }
            else
            {
                temp = temp.Where<BasePermission>(c => c.DeletionStateCode == 0);
            }
            permissionInfo.Total = temp.Count();
            return temp.OrderBy(c => c.SortCode).Skip(permissionInfo.PageSize * (permissionInfo.PageIndex - 1)).Take(permissionInfo.PageSize);
        }

        /// <summary>
        /// 实现删除权限的操作
        /// </summary>
        /// <param name="list">权限的集合信息</param>
        /// <returns>返回受影响的行数</returns>
        public int DeletePermission(List<int> list)
        {
            foreach (var ID in list)
            {
                _DbSession.BasePermissionRepository.DeleteEntity(new BasePermission()
                {
                    ID = ID
                });
            }
            return _DbSession.SaveChanges();
        }
    }
}