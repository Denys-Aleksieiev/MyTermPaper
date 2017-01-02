using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam_FinalProject_FileManager_DAL;

namespace Epam_FinalProject_FileManager_BLL.Interfaces
{
    public interface IUserService
    {
        ApplicationUser GetUserById(string id);
        long GetUserFreeSpace(string id);
    }
}
