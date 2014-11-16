  //引进TT模板的命名空间

//使用TT模板生成代码的片段
using System.Text;
using System.Threading.Tasks;
using LYZJ.UserLimitMVC.DAL;
using LYZJ.UserLimitMVC.IBLL;
using LYZJ.UserLimitMVC.IDAL;
using LYZJ.UserLimitMVC.Model;

namespace LYZJ.UserLimitMVC.BLL
{

	//在这里需要一个for循环来遍历数据库中所有的表放置在下面即可，这样就实现了所有的表对应的仓储显示出来了。
	 public partial class BasePermissionService : BaseService<BasePermission>, IBasePermissionService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BasePermissionRepository;
        }
	}
	 public partial class BasePermissionGroupService : BaseService<BasePermissionGroup>, IBasePermissionGroupService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BasePermissionGroupRepository;
        }
	}
	 public partial class BaseRoleService : BaseService<BaseRole>, IBaseRoleService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BaseRoleRepository;
        }
	}
	 public partial class BaseUserService : BaseService<BaseUser>, IBaseUserService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.BaseUserRepository;
        }
	}
	 public partial class R_Group_PermissionService : BaseService<R_Group_Permission>, IR_Group_PermissionService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.R_Group_PermissionRepository;
        }
	}
	 public partial class R_Group_RoleService : BaseService<R_Group_Role>, IR_Group_RoleService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.R_Group_RoleRepository;
        }
	}
	 public partial class R_Group_UserService : BaseService<R_Group_User>, IR_Group_UserService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.R_Group_UserRepository;
        }
	}
	 public partial class R_Role_PermissionService : BaseService<R_Role_Permission>, IR_Role_PermissionService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.R_Role_PermissionRepository;
        }
	}
	 public partial class R_User_PermissionService : BaseService<R_User_Permission>, IR_User_PermissionService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.R_User_PermissionRepository;
        }
	}
	 public partial class R_User_RoleService : BaseService<R_User_Role>, IR_User_RoleService
    {
		//只要想操作数据库，我们只要拿到DbSession就行
        public override void SetCurrentRepository()
        {
            CurrentRepository = _DbSession.R_User_RoleRepository;
        }
	}
}