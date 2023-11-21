using Microsoft.AspNetCore.Http;

namespace NetCore.BackendServer.Helpers
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile file);
    }
}
