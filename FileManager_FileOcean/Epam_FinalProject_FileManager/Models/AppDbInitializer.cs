using System.Data.Entity;
using System.IO;
using Epam_FinalProject_FileManager_DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Epam_FinalProject_FileManager.Models
{
    internal class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            var role1 = new IdentityRole {Name = "Admin"};
            var role2 = new IdentityRole {Name = "User"};

            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);

            // создаем пользователей
            var admin = new ApplicationUser {Email = "somemail@mail.ru", UserName = "somemail@mail.ru"};
            admin.UserStorageSize = 50073741824;
            string password = "111111b";
            string userFilesPath = System.AppDomain.CurrentDomain.BaseDirectory + "App_Data\\UserFiles\\";
            if (!Directory.Exists(userFilesPath + admin.Id + "/"))
            {
                Directory.CreateDirectory(userFilesPath + admin.Id + "/");
            }
            var result = userManager.Create(admin, password);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }

            base.Seed(context);
        }
    }
}
