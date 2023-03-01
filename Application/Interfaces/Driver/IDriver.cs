using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Application.Interfaces.Driver
{
    public interface IDriver
    {
        bool FileExists(string relpath);
        bool FolderExists(string relpath);
        IDirectoryContents GetRootFiles();
        IDirectoryContents GetFolderFiles(string relpath);
        IFileInfo GetFileInfo(string relpath);
        string CreateFolder(string relpath);
        Task CopyFile(string source, string target, CancellationToken cancellationToken);
        void RenameFolder(string oldname, string newname);
        void MoveFolder(string source, string target);
        Task MoveFile(string file, string folder, CancellationToken cancellationToken);
        Task<IFileInfo> UploadFile(IFormFile file, CancellationToken cancellationToken);
        Task<byte[]> DownloadFile(string filename, CancellationToken cancellationToken);
        void DeleteFile(string filename);
        void DeleteFolder(string path);
    }
}
