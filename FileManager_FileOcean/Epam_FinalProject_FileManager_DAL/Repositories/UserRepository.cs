using System.Linq;
using Epam_FinalProject_FileManager_DAL.Interfaces;

namespace Epam_FinalProject_FileManager_DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ApplicationUser> Users
        {
            get { return context.Users; }
        }

        public ApplicationUser GetUserById(string Id)
        {
            return context.Users.Find(Id);
        }
    }
}
