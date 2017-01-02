using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Epam_FinalProject_FileManager_DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<FileEntity> FileEntities { get; set; }
    }
}
