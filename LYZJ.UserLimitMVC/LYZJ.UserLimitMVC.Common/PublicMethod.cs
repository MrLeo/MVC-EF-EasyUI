using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.Common
{
    /// <summary>
    /// 存放公用的方法
    /// </summary>
    public class PublicMethod
    {
        public static Double AddDoubleData(Double num1, Double num2, Int32 count)
        {
            string result = (num1 + num2).ToString("N" + count.ToString());
            return Convert.ToDouble(result);
        }

        //产生一个6位数的随机数方法


    }
}
