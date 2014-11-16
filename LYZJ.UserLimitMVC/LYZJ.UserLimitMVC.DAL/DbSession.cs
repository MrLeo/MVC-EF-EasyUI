using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYZJ.UserLimitMVC.IDAL;

namespace LYZJ.UserLimitMVC.DAL
{
    //一次跟数据库交互的会话
    public partial class DbSession : IDbSession //代表应用程序跟数据库之间的一次会话，也是数据库访问层的统一入口
    {
        #region ----有模版自动生成----
        //public IDAL.IRoleRepository RoleRepository
        //{
        //    get { return new RoleRepository(); }
        //}

        //public IDAL.IUserInfoRepository UserInfoRepository
        //{
        //    get { return new UserInfoRepository(); }
        //} 
        #endregion

        /// <summary>
        /// 代表：当前应用程序跟数据库的会话内所有的实体的变化，更新会数据库
        /// </summary>
        /// <returns>返回受影响的行数</returns>
        public int SaveChanges()
        {
            //调用EF上下文的SaveChanges方法
            return DAL.EFContextFactory.GetCurrentDbContext().SaveChanges();
        }

        /// <summary>
        /// 执行Sql脚本的方法
        /// </summary>
        /// <param name="strSql">执行的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回受影响的行数</returns>
        public int ExcuteSql(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            //Ef4.0的执行方法 ObjectContext
            //封装一个执行SQl脚本的代码
            //return DAL.EFContextFactory.GetCurrentDbContext().ExecuteFunction(strSql, parameters);
            throw new NotImplementedException();
        }
    }
}
