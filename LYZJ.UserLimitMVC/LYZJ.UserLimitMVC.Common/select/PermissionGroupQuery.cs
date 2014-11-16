using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.Common.select
{
    public class PermissionGroupQuery : ParamterQuery
    {
        /// <summary>
        /// 菜单组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 菜单组类型
        /// </summary>
        public int? GroupType { get; set; }

        /// <summary>
        /// 删除信息(伪删除或者直接删除)
        /// </summary>
        public int? DeletionStateCode { get; set; }

    }
}
