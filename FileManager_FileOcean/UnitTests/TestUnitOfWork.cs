using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam_FinalProject_FileManager_DAL;
using Epam_FinalProject_FileManager_DAL.Interfaces;
using Epam_FinalProject_FileManager_DAL.Repositories;

namespace UnitTests
{
    
    class TestUnitOfWork : IUnitOfWork
    {
        IFileRepository _fileRepository = new TestFileRepository();
        public IFileRepository Files
        {
            get
            {
                return _fileRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
