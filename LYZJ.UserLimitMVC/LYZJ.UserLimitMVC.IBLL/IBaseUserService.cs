using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYZJ.UserLimitMVC.Model;
using LYZJ.UserLimitMVC.Common;
using LYZJ.UserLimitMVC.Common.select;

namespace LYZJ.UserLimitMVC.IBLL
{
    public partial interface IBaseUserService : IBaseService<BaseUser>
    {
        /// <summary>
        /// 在这里添加一个用户登录信息的约束
        /// </summary>
        /// <param name="userInfo">传递了用户的实体类</param>
        /// <returns>返回执行结果的枚举类型</returns>
        LoginResult CheckUserInfo(BaseUser userInfo);

        /// <summary>
        /// 验证用户名添加的时候不能重复
        /// </summary>
        /// <param name="UserName">得到参数用户名</param>
        /// <returns>返回执行结果的枚举类型</returns>
        LoginResult CheckUserNameTest(string UserName);

        /// <summary>
        /// 实现批量删除数据的方法
        /// </summary>
        /// <param name="deleteIds">批量删除数据的主键ID</param>
        /// <returns>返回受影响的行数</returns>
        int DeleteUsers(List<int> deleteIds);

        /// <summary>
        /// 实体数据类型的多条件查询
        /// </summary>
        /// <param name="query">将所有的条件全部封装到类里面，然后直接调用类</param>
        /// <returns>返回用户类的IQueryable集合</returns>
        IQueryable<BaseUser> LoadSearchData(UserInfoQuery query);


        /// <summary>
        /// 执行对用户设置角色的封装
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="roleIDs">角色集合的ID</param>
        /// <param name="userInfo">传递过去用户登录的session</param>
        /// <returns>返回是否执行成功的标志</returns>
        bool SetBaseUserRole(int userID, List<int> roleIDs, BaseUser userInfo);
    }
}
