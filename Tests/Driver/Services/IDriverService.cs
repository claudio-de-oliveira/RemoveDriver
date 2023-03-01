using Contracts.Entity.Driver;

using Microsoft.AspNetCore.Components.Forms;

namespace Driver.Services
{
    public interface IDriverService
    {
        Task<List<Models.FileInfoModel>> GetRootFilesInfo();
        Task<List<Models.FileInfoModel>> GetFolderFilesInfo(string subpath);
        Task<HttpResponseMessage> GetFileInfo(string file);
        Task<HttpResponseMessage> CopyFile(string sourceFile, string targetFile);
        Task<HttpResponseMessage> Rename(string oldname, string newname);
        Task<HttpResponseMessage> MoveFolder(string source, string target);
        Task<HttpResponseMessage> MoveFileToFolder(string sourceFile, string targetFolder);
        Task<HttpResponseMessage> DeleteFile(string path);
        Task<HttpResponseMessage> DeleteFolder(string folder);
        Task<HttpResponseMessage> CreateFolder(string folder);
        Task<HttpResponseMessage> UploadFile(IBrowserFile file, string relativePath);
        Task<FileContentResponse?> DownloadFile(string path);
    }
}
