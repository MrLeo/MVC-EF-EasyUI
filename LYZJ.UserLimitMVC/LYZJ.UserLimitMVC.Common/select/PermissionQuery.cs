using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.Common.select
{
    /// <summary>
    /// 对权限进行多条件的查询
    /// </summary>
    public class PermissionQuery : ParamterQuery
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string PerMission { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public int? RequestHttpType { get; set; }

        /// <summary>
        /// 是否有限
        /// </summary>
        public int? Enabled { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        public int? ActionType { get; set; }

        /// <summary>
        /// 删除信息(伪删除或者直接删除)
        /// </summary>
        public int? DeletionStateCode { get; set; }

    }
}
