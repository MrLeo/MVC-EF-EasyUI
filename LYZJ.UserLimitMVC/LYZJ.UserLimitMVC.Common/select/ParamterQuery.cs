using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.Common.select
{
    /// <summary>
    /// 定义参数的公用类
    /// </summary>
    public class ParamterQuery
    {
        /// <summary>
        /// 参数当前第几页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 参数每页显示多少条数据
        /// </summary>
        public int PageSize { get; set; }
        
        /// <summary>
        /// 总数是多少
        /// </summary>
        public int Total { get; set; }
    }
}
