using System;
using System.Collections.Generic;
using System.Linq;
using LYZJ.UserLimitMVC.IBLL;
using LYZJ.UserLimitMVC.Common;
using LYZJ.UserLimitMVC.Common.select;
using LYZJ.UserLimitMVC.Model;

namespace LYZJ.UserLimitMVC.BLL
{
    /// <summary>
    /// UserInfo业务逻辑
    /// </summary>
    public partial class BaseUserService : BaseService<BaseUser>, IBaseUserService
    {
        #region ---有系统自动生成---
        //只要想操作数据库，我们只要拿到DbSession就行
        //public override void SetCurrentRepository()
        //{
        //    CurrentRepository = _DbSession.UserInfoRepository;
        //} 
        #endregion

        #region ------注释掉的东西-------
        //访问DAL实现CRUD

        //private DAL.UserInfoRepository _userInfoRepository = new UserInfoRepository();

        //依赖接口编程
        //private IUserInfoRepository _userInfoRepository = new UserInfoRepository();

        //private IUserInfoRepository _userInfoRepository = RepositoryFactory.UserInfoRepository;

        //public  UserInfo AddUserInfo(UserInfo userInfo)
        //{
        //    return _userInfoRepository.AddEntity(userInfo);
        //}

        //public bool UpdateUserInfo(UserInfo userInfo)
        //{
        //    return _userInfoRepository.UpdateEntity(userInfo);
        //} 
        #endregion

        /// <summary>
        /// 完成了对用户的校验
        /// </summary>
        /// <param name="userInfo">传递了用户类</param>
        /// <returns>返回了执行结果的枚举类型</returns>
        public LoginResult CheckUserInfo(BaseUser userInfo)
        {
            //首先判断用户名，密码是否为空
            if (string.IsNullOrEmpty(userInfo.UserName))
            {
                return LoginResult.UserIsNull;
            }
            if (string.IsNullOrEmpty(userInfo.UserPassword))
            {
                return LoginResult.PwdIsNUll;
            }
            //如果不为空的话则去数据库中查询信息


            //在这里会去数据库检查是否有数据，如果没有的话就会返回一个空值
            var LoginUserInfoCheck = _DbSession.BaseUserRepository.LoadEntities(u => u.UserName == userInfo.UserName && u.AuditStatus == "已审核" && u.Enabled == 1).FirstOrDefault();
            //对返回的结果进行判断
            if (LoginUserInfoCheck == null)
            {
                return LoginResult.UserNotExist;
            }
            if (LoginUserInfoCheck.UserPassword != userInfo.UserPassword)
            {
                return LoginResult.PwdError;
            }
            else
            {
                return LoginResult.OK;
            }
        }

        /// <summary>
        /// 判断用户名不能重复
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回执行结果的枚举类型</returns>
        public LoginResult CheckUserNameTest(string UserName)
        {
            //首先判断是否为空
            if (String.IsNullOrEmpty(UserName))
            {
                return LoginResult.UserIsNull;
            }
            var checkUserName = _DbSession.BaseUserRepository.LoadEntities(u => u.UserName == UserName).FirstOrDefault();
            if (checkUserName != null)
            {
                return LoginResult.UserExist;
            }
            else
            {
                return LoginResult.OK;
            }
        }

        /// <summary>
        /// 实现批量删除数据
        /// </summary>
        /// <param name="deleteIds">批量删除的List集合的参数</param>
        /// <returns>返回受影响的行数</returns>
        public int DeleteUsers(List<int> deleteIds)
        {
            foreach (var deleteID in deleteIds)
            {
                _DbSession.BaseUserRepository.DeleteEntity(new BaseUser()
                {
                    ID = deleteID
                });
            }
            return _DbSession.SaveChanges();
        }

        /// <summary>
        /// 实现对多条件查询的判断方法的封装
        /// </summary>
        /// <param name="query">引用传递，传递参数的信息</param>
        /// <returns>返回用户类的IQueryable集合</returns>
        public IQueryable<BaseUser> LoadSearchData(UserInfoQuery query)
        {
            var temp = _DbSession.BaseUserRepository.LoadEntities(u => true);
            //首先过滤姓名
            if (!string.IsNullOrEmpty(query.RealName))
            {
                temp = temp.Where<BaseUser>(u => u.RealName.Contains(query.RealName));  //like '%mmm%'
            }
            if (!string.IsNullOrEmpty(query.Telephone))
            {
                temp = temp.Where<BaseUser>(u => u.Telephone.Contains(query.Telephone));
            }
            if (!string.IsNullOrEmpty(query.EMail))
            {
                temp = temp.Where<BaseUser>(u => u.Email.Contains(query.EMail));
            }
            if (query.Enabled != -1)
            {
                temp = temp.Where<BaseUser>(u => u.Enabled == query.Enabled);
            }
            if (!string.IsNullOrEmpty(query.AuditStatus) && query.AuditStatus != "-1")
            {
                temp = temp.Where<BaseUser>(u => u.AuditStatus.Contains(query.AuditStatus));
            }
            temp = query.DeletionStateCod == 1 ? temp.Where<BaseUser>(u => u.DeletionStateCode == query.DeletionStateCod) : temp.Where<BaseUser>(u => u.DeletionStateCode == 0);
            query.Total = temp.Count();
            return temp.OrderBy(u => u.SortCode).Skip(query.PageSize * (query.PageIndex - 1)).Take(query.PageSize);
        }

        /// <summary>
        /// 执行对用户设置角色的封装
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="roleIDs">角色集合的ID</param>
        /// <param name="userInfo">传递过去用户登录的session</param>
        /// <returns>返回是否执行成功的标志</returns>
        public bool SetBaseUserRole(int userID, List<int> roleIDs, BaseUser userInfo)
        {
            //首先根据传递过来的userID判断用户是否存在
            var currentUserInfo = _DbSession.BaseUserRepository.LoadEntities(c => c.ID == userID).FirstOrDefault();
            if (currentUserInfo == null)
            {
                return false;
            }
            //首先获取到角色表中的所有信息返回
            var listRoles = currentUserInfo.R_UserInfo_Role.ToList();
            foreach (var t in listRoles)
            {
                _DbSession.R_User_RoleRepository.DeleteEntity(t);
            }
            //真正的删除了这个用户下面的所有的数据
            _DbSession.SaveChanges();
            //然后重新给这个用户赋予权限
            foreach (var roleID in roleIDs)
            {
                //给用户批量插入角色，在中间表中,这里需要改成一个批量提交添加数据的
                var rUserInfoRole = new R_User_Role
                                        {
                                            RoleID = roleID,
                                            UserID = userID,
                                            CreateOn = DateTime.Parse(DateTime.Now.ToString())
                                        };
                var user = userInfo;
                rUserInfoRole.CreateUserID = user.Code;
                rUserInfoRole.CreateBy = user.UserName;
                _DbSession.R_User_RoleRepository.AddEntity(rUserInfoRole);
            }
           //执行真正的添加
            _DbSession.SaveChanges();
            return true;
        }
    }
}
