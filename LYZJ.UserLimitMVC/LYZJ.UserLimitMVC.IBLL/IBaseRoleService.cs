using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYZJ.UserLimitMVC.Model;
using LYZJ.UserLimitMVC.Common.select;

namespace LYZJ.UserLimitMVC.IBLL
{
    public partial interface IBaseRoleService : IBaseService<BaseRole>
    {
        /// <summary>
        /// 实现条件查询
        /// </summary>
        /// <param name="roleinfo">传递过来的多条件查询的信息</param>
        /// <returns>返回实现的查询的条件</returns>
        IQueryable<BaseRole> loadSearchDate(RoleInfoQuery roleinfo);

        /// <summary>
        /// 实现批量删除数据
        /// </summary>
        /// <param name="list">要删除的数据的集合</param>
        /// <returns>返回受影响的行数</returns>
        int DeleteRoles(List<int> list);
    }
}
