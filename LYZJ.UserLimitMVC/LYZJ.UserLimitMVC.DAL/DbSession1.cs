  //引进TT模板的命名空间

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


	//在这里需要一个for循环来遍历数据库中所有的表放置在下面即可，这样就实现了所有的表对应的仓储显示出来了。
	        public IDAL.IBasePermissionRepository BasePermissionRepository
        {
            get { return new BasePermissionRepository(); }
        }
		        public IDAL.IBasePermissionGroupRepository BasePermissionGroupRepository
        {
            get { return new BasePermissionGroupRepository(); }
        }
		        public IDAL.IBaseRoleRepository BaseRoleRepository
        {
            get { return new BaseRoleRepository(); }
        }
		        public IDAL.IBaseUserRepository BaseUserRepository
        {
            get { return new BaseUserRepository(); }
        }
		        public IDAL.IR_Group_PermissionRepository R_Group_PermissionRepository
        {
            get { return new R_Group_PermissionRepository(); }
        }
		        public IDAL.IR_Group_RoleRepository R_Group_RoleRepository
        {
            get { return new R_Group_RoleRepository(); }
        }
		        public IDAL.IR_Group_UserRepository R_Group_UserRepository
        {
            get { return new R_Group_UserRepository(); }
        }
		        public IDAL.IR_Role_PermissionRepository R_Role_PermissionRepository
        {
            get { return new R_Role_PermissionRepository(); }
        }
		        public IDAL.IR_User_PermissionRepository R_User_PermissionRepository
        {
            get { return new R_User_PermissionRepository(); }
        }
		        public IDAL.IR_User_RoleRepository R_User_RoleRepository
        {
            get { return new R_User_RoleRepository(); }
        }
		}
}