  //引进TT模板的命名空间

//使用TT模板生成代码的片段
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYZJ.UserLimitMVC.Model;

namespace LYZJ.UserLimitMVC.IBLL
{

	//在这里需要一个for循环来遍历数据库中所有的表放置在下面即可，这样就实现了所有的表对应的仓储显示出来了。
	 public partial interface IBasePermissionService:IBaseService<BasePermission>
    {
	}
	 public partial interface IBasePermissionGroupService:IBaseService<BasePermissionGroup>
    {
	}
	 public partial interface IBaseRoleService:IBaseService<BaseRole>
    {
	}
	 public partial interface IBaseUserService:IBaseService<BaseUser>
    {
	}
	 public partial interface IR_Group_PermissionService:IBaseService<R_Group_Permission>
    {
	}
	 public partial interface IR_Group_RoleService:IBaseService<R_Group_Role>
    {
	}
	 public partial interface IR_Group_UserService:IBaseService<R_Group_User>
    {
	}
	 public partial interface IR_Role_PermissionService:IBaseService<R_Role_Permission>
    {
	}
	 public partial interface IR_User_PermissionService:IBaseService<R_User_Permission>
    {
	}
	 public partial interface IR_User_RoleService:IBaseService<R_User_Role>
    {
	}
}