using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam_FinalProject_FileManager_DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IFileRepository Files { get; }
        IUserRepository Users { get; }
        void Save();
    }
}
