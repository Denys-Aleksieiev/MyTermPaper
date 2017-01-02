using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam_FinalProject_FileManager_DAL.Interfaces;
using Epam_FinalProject_FileManager_DAL.Repositories;

namespace Epam_FinalProject_FileManager_DAL
{
    public class EFUnitOfWork : IUnitOfWork
    {
        ApplicationDbContext context;
        private FileRepository fileRepository;
        private UserRepository userRepository;


        public EFUnitOfWork()
        {
            context = new ApplicationDbContext();
        }
        public IFileRepository Files
        {
            get
            {
                if (fileRepository == null)
                {
                    fileRepository = new FileRepository(context);
                }
                return fileRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(context);
                }
                return userRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
