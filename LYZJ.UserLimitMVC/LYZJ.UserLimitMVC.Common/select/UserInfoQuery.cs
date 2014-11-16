using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.Common.select
{
    /// <summary>
    /// 对多条件查询的其他参数的录入
    /// </summary>
    public class UserInfoQuery : ParamterQuery
    {
        /// <summary>
        /// 真实的姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Email邮件
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// 此用户是否有效
        /// </summary>
        public int? Enabled { get; set; }

        /// <summary>
        /// 用户的状态
        /// </summary>
        public string AuditStatus { get; set; }

        /// <summary>
        /// 删除的标致
        /// </summary>
        public int? DeletionStateCod { get; set; }
    }
}
