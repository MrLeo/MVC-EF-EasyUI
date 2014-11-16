using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.Common.Enum
{
    /// <summary>
    /// 用户是否有效的枚举类
    /// </summary>
    public enum EnabledEnum
    {
        /// <summary>
        /// 没有作用，登录不了
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 有效
        /// </summary>
        OK = 1,
    }
}
