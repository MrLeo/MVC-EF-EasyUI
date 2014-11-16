using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.Common.Enum
{
    /// <summary>
    /// 是否伪删除的枚举类
    /// </summary>
    public enum DeletionStateCodeEnum
    {
        /// <summary>
        /// 默认没有删除
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 默认删除
        /// </summary>
        OK = 1,
    }
}
