using Application.Interfaces.Driver;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Infrastructure.Services.Driver
{
    // ***************************************************************
    // OBS: As excessões são tratadas na camada de Aplicação
    // ***************************************************************

    public class Driver : IDriver
    {
        private readonly IFileProvider _fileProvider;
        private readonly string _root = "";

        public Driver(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
            if (_fileProvider is PhysicalFileProvider physicalFileProvider)
            {
                _root = physicalFileProvider.Root;
                if (!Directory.Exists(_root))
                    Directory.CreateDirectory(_root);
            }
        }

        public bool FileExists(string relpath)
        {
            var fileInfo = _fileProvider.GetFileInfo(RelativePath(relpath));
            return File.Exists(fileInfo.PhysicalPath);
        }
        public bool FolderExists(string relpath)
        {
            var fileInfo = _fileProvider.GetFileInfo(RelativePath(relpath));
            return Directory.Exists(fileInfo.PhysicalPath);
        }
        public IDirectoryContents GetRootFiles()
            => _fileProvider.GetDirectoryContents("");
        public IDirectoryContents GetFolderFiles(string relpath)
            => _fileProvider.GetDirectoryContents(RelativePath(relpath));
        public IFileInfo GetFileInfo(string relpath)
            => _fileProvider.GetFileInfo(RelativePath(relpath));

        public string CreateFolder(string relpath)
        {
            var source = _fileProvider.GetFileInfo(RelativePath(relpath));
            if (!Directory.Exists(source.PhysicalPath))
                Directory.CreateDirectory(source.PhysicalPath!);
            return source.PhysicalPath!;
        }

        public async Task CopyFile(string src, string trg, CancellationToken cancellationToken)
        {
            var fileName = Path.GetFileName(src);
            var source = _fileProvider.GetFileInfo(RelativePath(src));
            var target = _fileProvider.GetFileInfo(RelativePath(trg));
            if (source.PhysicalPath! != Path.Combine(target.PhysicalPath!, fileName))
                await Copy(source.PhysicalPath!, Path.Combine(target.PhysicalPath!, fileName), cancellationToken);
        }

        public void RenameFolder(string oldname, string newname)
        {
            var sourceInfo = _fileProvider.GetFileInfo(RelativePath(oldname));
            var targetInfo = _fileProvider.GetFileInfo(RelativePath(newname));
            if (sourceInfo.PhysicalPath! != targetInfo.PhysicalPath!)
                Directory.Move(sourceInfo.PhysicalPath!, targetInfo.PhysicalPath!);
        }

        public void MoveFolder(string source, string target)
        {
            var sourceInfo = _fileProvider.GetFileInfo(RelativePath(source));
            var targetInfo = _fileProvider.GetFileInfo(RelativePath(target));
            if (sourceInfo.PhysicalPath! != targetInfo.PhysicalPath! && !Directory.Exists(targetInfo.PhysicalPath))
                Directory.Move(sourceInfo.PhysicalPath!, targetInfo.PhysicalPath!);
        }

        public async Task MoveFile(string file, string folder, CancellationToken cancellationToken)
        {
            var fileName = Path.GetFileName(file);
            var fullSourceFile = _fileProvider.GetFileInfo(RelativePath(file));
            var fullTargetFile = _fileProvider.GetFileInfo(Path.Combine(RelativePath(folder), RelativePath(fileName)));

            if (fullSourceFile.PhysicalPath! != fullTargetFile.PhysicalPath!)
            {
                await Copy(fullSourceFile.PhysicalPath!, fullTargetFile.PhysicalPath!, cancellationToken);
                var info = new FileInfo(fullSourceFile.PhysicalPath!);
                info.Delete();
            }
        }

        public async Task<IFileInfo> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            var source = _fileProvider.GetFileInfo(RelativePath(file.FileName));

            if (file.Length > 0)
            {
                using Stream fileStream = new FileStream(source.PhysicalPath!, FileMode.Create);
                await file.CopyToAsync(fileStream, cancellationToken);
            }

            return _fileProvider.GetFileInfo(RelativePath(file.FileName));
        }

        public async Task<byte[]> DownloadFile(string filename, CancellationToken cancellationToken)
        {
            var source = _fileProvider.GetFileInfo(RelativePath(filename));
            using var stream = new FileStream(source.PhysicalPath!, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[stream.Length];
            _ = await stream.ReadAsync(buffer, cancellationToken);
            return buffer;
        }

        public void DeleteFile(string filename)
        {
            var source = _fileProvider.GetFileInfo(RelativePath(filename));
            var info = new FileInfo(source.PhysicalPath!);
            info.Delete();
        }

        public void DeleteFolder(string path)
        {
            var source = _fileProvider.GetFileInfo(RelativePath(path));
            Directory.Delete(source.PhysicalPath!);
        }

        //\//\//\//\//\//\//\//\//\//\//\//\//\//\//\//\//\//\//\//\//\//\
        private string RelativePath(string relpath)
        {
            if (relpath.StartsWith(_root))
                return relpath[_root.Length..];
            return relpath;
        }

        private static async Task Copy(string sourceFile, string targetFile, CancellationToken cancellationToken)
        {
            using var stream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
            using var fileStream = new FileStream(targetFile, FileMode.Create);
            await stream.CopyToAsync(fileStream, cancellationToken);
        }
    }
}