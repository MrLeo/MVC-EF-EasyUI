using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.Common
{
    /// <summary>
    /// 枚举出当用户登录的时候出现的各种错误
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// 密码错误
        /// </summary>
        PwdError,   

        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotExist,

        /// <summary>
        /// 用户名为空
        /// </summary>
        UserIsNull,

        /// <summary>
        /// 密码为空
        /// </summary>
        PwdIsNUll,

        /// <summary>
        /// 登录成功
        /// </summary>
        OK,

        /// <summary>
        /// 用户名已存在
        /// </summary>
        UserExist, 
    }
}