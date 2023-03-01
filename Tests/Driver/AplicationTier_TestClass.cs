using Application.Handlers.Driver.Create;
using Application.Interfaces.Driver;

using Microsoft.Extensions.FileProviders;

namespace Driver
{
    [TestClass()]
    public class AplicationTier_TestClass
    {
        private static readonly IDriver _driver =
            new Infrastructure.Services.Driver.Driver(
                new PhysicalFileProvider(
                    Environment.ExpandEnvironmentVariables(
                        @"%USERPROFILE%\Driver"
                        )
                    )
                );

        [TestMethod()]
        public void CreateFolderCommandHandlerSuccess()
        {
            var createFolderCommandHandler = new CreateFolderCommandHandler(_driver);

            var result = createFolderCommandHandler.Handle(
                new CreateFolderCommand("Folder"),
                CancellationToken.None
                ).GetAwaiter().GetResult();

            Assert.IsFalse(result.IsError);
            string path = result.Value;
            Assert.IsTrue(path.EndsWith("\\Folder"));
        }

        [TestMethod()]
        public void CreateFolderCommandHandlerFail()
        {
            var createFolderCommandHandler = new CreateFolderCommandHandler(_driver);

            var result = createFolderCommandHandler.Handle(
                new CreateFolderCommand("/?Folder"), // Invalid folder
                CancellationToken.None
                ).GetAwaiter().GetResult();

            Assert.IsTrue(result.IsError);
            Assert.IsTrue(result.FirstError.Code == "Driver.Exception");
        }

        [TestMethod]
        public void UploadFileCommandHandler()
        {
            // Todo
        }

        [TestMethod]
        public void DeleteFileCommandHandler()
        {
            // Todo
        }

        [TestMethod]
        public void DeleteFolderCommandHandler()
        {
            // Todo
        }

        [TestMethod]
        public void DownloadFileQueryHandler()
        {
            // Todo
        }

        [TestMethod]
        public void GetFileInfoQueryHandler()
        {
            // Todo
        }

        [TestMethod]
        public void GetFilesQueryHandler()
        {
            // Todo
        }

        [TestMethod]
        public void GetFolderFilesQueryHandler()
        {
            // Todo
        }

        [TestMethod]
        public void CopyFileCommandHandler()
        {
            // Todo
        }

        [TestMethod]
        public void MoveFileCommandHandler()
        {
            // Todo
        }

        [TestMethod]
        public void MoveFolderCommandHandler()
        {
            // Todo
        }

        [TestMethod]
        public void RenameFolderCommandHandler()
        {
            // Todo
        }
    }
}
