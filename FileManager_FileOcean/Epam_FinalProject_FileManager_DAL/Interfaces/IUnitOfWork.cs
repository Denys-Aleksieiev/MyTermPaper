namespace Epam_FinalProject_FileManager_DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IFileRepository Files { get; }
        IUserRepository Users { get; }
        void Save();
    }
}
