using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage
{
    public interface IStorageService
    {
        string[] ListFiles();
        Task<Stream> ReadFile(string fileName);
        void StoreData(string data, string fileName);
        string ReadFileAsString(string fileName);
    }
}
