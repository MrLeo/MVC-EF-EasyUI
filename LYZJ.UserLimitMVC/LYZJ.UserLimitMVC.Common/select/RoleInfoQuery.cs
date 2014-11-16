using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.Common.select
{
    /// <summary>
    /// 对角色进行多条件查询的条件
    /// </summary>
    public class RoleInfoQuery : ParamterQuery
    {
        /// <summary>
        /// 角色的名字
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 角色是否有效
        /// </summary>
        public int? Enabled { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public int? CategoryCode { get; set; }

        /// <summary>
        /// 是否删除的标志
        /// </summary>
        public int? DeletionStateCode { get; set; }
    }
}
