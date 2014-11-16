  //引进TT模板的命名空间

//使用TT模板生成代码的片段
using LYZJ.UserLimitMVC.IDAL;
using LYZJ.UserLimitMVC.Model;

namespace LYZJ.UserLimitMVC.DAL
{


	//在这里需要一个for循环来遍历数据库中所有的表放置在下面即可，这样就实现了所有的表对应的仓储显示出来了。
	    public partial class BasePermissionRepository : BaseRepository<BasePermission>, IBasePermissionRepository
    {

    }

	    public partial class BasePermissionGroupRepository : BaseRepository<BasePermissionGroup>, IBasePermissionGroupRepository
    {

    }

	    public partial class BaseRoleRepository : BaseRepository<BaseRole>, IBaseRoleRepository
    {

    }

	    public partial class BaseUserRepository : BaseRepository<BaseUser>, IBaseUserRepository
    {

    }

	    public partial class R_Group_PermissionRepository : BaseRepository<R_Group_Permission>, IR_Group_PermissionRepository
    {

    }

	    public partial class R_Group_RoleRepository : BaseRepository<R_Group_Role>, IR_Group_RoleRepository
    {

    }

	    public partial class R_Group_UserRepository : BaseRepository<R_Group_User>, IR_Group_UserRepository
    {

    }

	    public partial class R_Role_PermissionRepository : BaseRepository<R_Role_Permission>, IR_Role_PermissionRepository
    {

    }

	    public partial class R_User_PermissionRepository : BaseRepository<R_User_Permission>, IR_User_PermissionRepository
    {

    }

	    public partial class R_User_RoleRepository : BaseRepository<R_User_Role>, IR_User_RoleRepository
    {

    }

}