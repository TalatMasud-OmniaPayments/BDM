using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.DataAccess.Interface.Entities;

namespace Omnia.Pie.Vtm.DataAccess.Interface
{
    public interface IUserRolesStore
    {
        Task ClearAll();
        UserRoles GetUserRole(String username);
        Task Update(UserRoles userRoles);
        
        //Task<List<Role>> GetList();
        //Task Save(Role roleType);
    }
}
