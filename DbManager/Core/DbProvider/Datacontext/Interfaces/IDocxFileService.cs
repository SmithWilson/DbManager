using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface IDocxFileService
    {
        Task PutDocxFileToDatabase(int id, string path);

        Task GetDoxcFileFromDatabase(int id, string fileName);
    }
}
