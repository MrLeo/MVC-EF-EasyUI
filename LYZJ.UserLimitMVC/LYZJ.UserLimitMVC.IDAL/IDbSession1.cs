  //引进TT模板的命名空间

//使用TT模板生成代码的片段
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.IDAL
{
    public partial interface IDbSession
    {

	//在这里需要一个for循环来遍历数据库中所有的表放置在下面即可，这样就实现了所有的表对应的仓储显示出来了。
	   //每个表对应的实体仓储对象
        IDAL.IBasePermissionRepository BasePermissionRepository { get; }

		   //每个表对应的实体仓储对象
        IDAL.IBasePermissionGroupRepository BasePermissionGroupRepository { get; }

		   //每个表对应的实体仓储对象
        IDAL.IBaseRoleRepository BaseRoleRepository { get; }

		   //每个表对应的实体仓储对象
        IDAL.IBaseUserRepository BaseUserRepository { get; }

		   //每个表对应的实体仓储对象
        IDAL.IR_Group_PermissionRepository R_Group_PermissionRepository { get; }

		   //每个表对应的实体仓储对象
        IDAL.IR_Group_RoleRepository R_Group_RoleRepository { get; }

		   //每个表对应的实体仓储对象
        IDAL.IR_Group_UserRepository R_Group_UserRepository { get; }

		   //每个表对应的实体仓储对象
        IDAL.IR_Role_PermissionRepository R_Role_PermissionRepository { get; }

		   //每个表对应的实体仓储对象
        IDAL.IR_User_PermissionRepository R_User_PermissionRepository { get; }

		   //每个表对应的实体仓储对象
        IDAL.IR_User_RoleRepository R_User_RoleRepository { get; }

		}
}