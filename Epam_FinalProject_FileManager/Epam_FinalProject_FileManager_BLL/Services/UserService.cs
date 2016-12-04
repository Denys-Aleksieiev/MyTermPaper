using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam_FinalProject_FileManager_BLL.Interfaces;
using Epam_FinalProject_FileManager_DAL;
using Epam_FinalProject_FileManager_DAL.Interfaces;

namespace Epam_FinalProject_FileManager_BLL.Services
{
    public class UserService : IUserService
    {
        readonly IUnitOfWork _database;
        public UserService(IUnitOfWork database)
        {
            _database = database;
        }
        public ApplicationUser GetUserById(string id)
        {
            return _database.Users.GetUserById(id);
        }
        public long GetUserFreeSpace(string id)
        {
            ApplicationUser user = _database.Users.GetUserById(id);
            return user.UserStorageSize - user.UserFiles.Sum(f => f.Size);
        }
    }
}
