using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYZJ.UserLimitMVC.IBLL;
using LYZJ.UserLimitMVC.Model;
using LYZJ.UserLimitMVC.Common.select;

namespace LYZJ.UserLimitMVC.BLL
{
    public partial class BaseRoleService : BaseService<BaseRole>, IBaseRoleService
    {

        #region ----有系统自动生成----
        //重写抽象方法，设置当前仓储为Role仓储
        //public override void SetCurrentRepository()
        //{
        //    //设置当前仓储为Role仓储
        //    CurrentRepository = _DbSession.RoleRepository;
        //} 
        #endregion

        /// <summary>
        /// 实现条件查询
        /// </summary>
        /// <param name="roleinfo">传递过来的多条件查询的信息</param>
        /// <returns>返回实现的查询的条件</returns>
        public IQueryable<BaseRole> loadSearchDate(RoleInfoQuery roleinfo)
        {
            var temp = _DbSession.BaseRoleRepository.LoadEntities(c => true);
            if (!string.IsNullOrEmpty(roleinfo.RealName))
            {
                temp = temp.Where<BaseRole>(u => u.Realname.Contains(roleinfo.RealName));
            }
            if (roleinfo.Enabled != -1)
            {
                temp = temp.Where<BaseRole>(u => u.Enabled == roleinfo.Enabled);
            }
            if (roleinfo.CategoryCode != -1)
            {
                temp = temp.Where<BaseRole>(u => u.CategoryCode == roleinfo.CategoryCode);
            }
            if (roleinfo.DeletionStateCode == 1)
            {
                temp = temp.Where<BaseRole>(u => u.DeletionStateCode ==roleinfo.DeletionStateCode);
            }
            else
            {
                temp = temp.Where<BaseRole>(u => u.DeletionStateCode == 0);
            }
            roleinfo.Total = temp.Count();
            return temp.OrderBy(u => u.SortCode).Skip(roleinfo.PageSize * (roleinfo.PageIndex - 1)).Take(roleinfo.PageSize);

        }

        /// <summary>
        /// 实现批量删除数据
        /// </summary>
        /// <param name="list">要删除的数据的集合</param>
        /// <returns>返回受影响的行数</returns>
        public int DeleteRoles(List<int> list)
        {
            foreach (var ID in list)
            {
                _DbSession.BaseRoleRepository.DeleteEntity(new BaseRole()
                {
                    ID = ID
                });
            }
            return _DbSession.SaveChanges();
        }
    }
}
