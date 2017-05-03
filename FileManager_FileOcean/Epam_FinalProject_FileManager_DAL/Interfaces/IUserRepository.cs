using System.Linq;

namespace Epam_FinalProject_FileManager_DAL.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> Users { get; }
        ApplicationUser GetUserById(string Id);
    }
}
