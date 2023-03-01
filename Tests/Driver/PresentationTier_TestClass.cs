using Driver.Services;

namespace Driver
{
    [TestClass()]
    public class PresentationTier_TestClass
    {
        private readonly IDriverService _driverService;

        public PresentationTier_TestClass()
        {
            _driverService = new DriverService("http://localhost:5253/driver/");
        }

        [TestMethod()]
        public void CreateFolder_GetRootFiles_DeleteFolder_Success_Test()
        {
            var folder = Guid.NewGuid().ToString();

            var result1 = _driverService.CreateFolder(folder).GetAwaiter().GetResult();
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result2 = _driverService.GetRootFilesInfo().GetAwaiter().GetResult();
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result2.Count > 0);
            Task.Delay(100).GetAwaiter().GetResult();

            var result3 = _driverService.DeleteFolder(folder).GetAwaiter().GetResult();
            Assert.IsNotNull(result3);
            Assert.IsTrue(result3.IsSuccessStatusCode);
        }

        [TestMethod()]
        public void GetFolderFilesSuccessTest()
        {
            var folder1 = Guid.NewGuid().ToString();
            var folder2 = $"{folder1}/{Guid.NewGuid()}";

            var result1 = _driverService.CreateFolder(folder1).GetAwaiter().GetResult();
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result2 = _driverService.CreateFolder(folder2).GetAwaiter().GetResult();
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result3 = _driverService.GetFolderFilesInfo(folder2).GetAwaiter().GetResult();
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result3.Count == 1);
            Task.Delay(100).GetAwaiter().GetResult();

            var result4 = _driverService.DeleteFolder(folder2).GetAwaiter().GetResult();
            Assert.IsNotNull(result4);
            Assert.IsTrue(result4.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result5 = _driverService.DeleteFolder(folder1).GetAwaiter().GetResult();
            Assert.IsNotNull(result5);
            Assert.IsTrue(result5.IsSuccessStatusCode);
        }

        [TestMethod()]
        public void GetFileInfoNotFoundFailureTest()
        {
            var result = _driverService.GetFileInfo(Guid.NewGuid().ToString()).GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod()]
        public void CreateMultiFoldersTest()
        {
            var folder1 = Guid.NewGuid().ToString();
            var folder2 = $"{folder1}/{Guid.NewGuid()}";

            var result1 = _driverService.CreateFolder(folder2).GetAwaiter().GetResult();
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result2 = _driverService.DeleteFolder(folder2).GetAwaiter().GetResult();
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result3 = _driverService.DeleteFolder(folder1).GetAwaiter().GetResult();
            Assert.IsNotNull(result3);
            Assert.IsTrue(result3.IsSuccessStatusCode);
        }

        [TestMethod()]
        public void RenameFolder_MoveFolder_Success_Test()
        {
            var folder1 = Guid.NewGuid().ToString();
            var folder2 = $"{folder1}/{Guid.NewGuid()}";

            var result1 = _driverService.CreateFolder(folder2).GetAwaiter().GetResult();
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result2 = _driverService.Rename(folder2, $"{folder1}/folder").GetAwaiter().GetResult();
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result5 = _driverService.MoveFolder($"{folder1}/folder", "folder").GetAwaiter().GetResult();
            Assert.IsNotNull(result5);
            Assert.IsTrue(result5.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result8 = _driverService.DeleteFolder("folder").GetAwaiter().GetResult();
            Assert.IsNotNull(result8);
            Assert.IsTrue(result8.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result9 = _driverService.DeleteFolder(folder1).GetAwaiter().GetResult();
            Assert.IsNotNull(result9);
            Assert.IsTrue(result9.IsSuccessStatusCode);
        }

        [TestMethod()]
        public void RenameFolder_MoveFolder_Failure_Test()
        {
            var folder1 = Guid.NewGuid().ToString();
            var folder2 = $"{folder1}/{Guid.NewGuid()}";

            var result1 = _driverService.CreateFolder(folder2).GetAwaiter().GetResult();
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result2 = _driverService.Rename(folder2, $"xxx/folder").GetAwaiter().GetResult();
            Assert.IsNotNull(result2);
            Assert.IsFalse(result2.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result5 = _driverService.MoveFolder(folder2, "xxx/folder").GetAwaiter().GetResult();
            Assert.IsNotNull(result5);
            Assert.IsFalse(result5.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result8 = _driverService.DeleteFolder(folder2).GetAwaiter().GetResult();
            Assert.IsNotNull(result8);
            Assert.IsTrue(result8.IsSuccessStatusCode);
            Task.Delay(100).GetAwaiter().GetResult();

            var result9 = _driverService.DeleteFolder(folder1).GetAwaiter().GetResult();
            Assert.IsNotNull(result9);
            Assert.IsTrue(result9.IsSuccessStatusCode);
        }

        [TestMethod()]
        public void CopyFileTest()
        {
            // Todo
        }

        [TestMethod()]
        public void MoveFileTest()
        {
            // Todo
        }

        [TestMethod()]
        public void UploadFileTest()
        {
            // Todo
        }

        [TestMethod()]
        public void DownloadFileTest()
        {
            // Todo
        }

        [TestMethod()]
        public void DeleteFileTest()
        {
            // Todo
        }
    }
}
