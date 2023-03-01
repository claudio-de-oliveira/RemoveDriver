using Application.Interfaces.Driver;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace Driver
{
    [TestClass()]
    public class InfrastructureTier_TestClass
    {
        private static readonly IDriver _driver =
            new Infrastructure.Services.Driver.Driver(
                new PhysicalFileProvider(
                    Environment.ExpandEnvironmentVariables(
                        @"%USERPROFILE%\Driver"
                        )
                    )
                );

        // ******************************************************************
        // A camada de INFRASTRUTURA não trata excessões!
        // ******************************************************************

        private static void UploadFile(IDriver driver, string fileName)
        {
            using var stream = new MemoryStream();
            stream.WriteByte(57);

            IFormFile formFile = new FormFile(stream, 0, 1, "MyFormFile", fileName);
            driver.UploadFile(formFile, CancellationToken.None);
        }

        [TestMethod()]
        public void PhysicalDriverTest()
        {
            Assert.IsNotNull(_driver);
        }

        [TestMethod()]
        public void CreateFolder_ExistsFolder_UploadFile_DeleteFile_DeleteFolder_Test()
        {
            var folderName = "Folder";
            string fileName = Path.Combine(folderName, "temp.dat");

            // Criação de folder
            _driver.CreateFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName), "CreateFolder NOVO falhou!");

            // Criação de folder já existente => não faz nada
            _driver.CreateFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName), "CreateFolder EXISTENTE falhou!");

            // Cria um arquivo dentro do folter 
            UploadFile(_driver, fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists(fileName), "UploadFile falhou!");

            // Exclusão de arquivo
            _driver.DeleteFile(fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists(fileName), "DeleteFile falhou!");

            // Exclusão de folder vazio
            _driver.DeleteFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(folderName), "DeleteFolder falhou!");
        }

        [TestMethod()]
        public void GetFiles_GetFolderFiles_Test()
        {
            // A pasta Root deve conter dados
            var contents = _driver.GetRootFiles();
            Assert.IsTrue(contents.Any());

            var folderName = "Folder";
            string fileName = Path.Combine(folderName, "temp.dat");

            // Criação de folder
            _driver.CreateFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName));

            // A pasta criada não deve conter dados
            contents = _driver.GetFolderFiles(folderName);
            Assert.IsFalse(contents.Any());

            // Cria um arquivo dentro do folter 
            UploadFile(_driver, fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists(fileName));

            // A pasta criada agora deve conter dados
            contents = _driver.GetFolderFiles(folderName);
            Assert.IsTrue(contents.Any());

            // Exclusão de arquivo
            _driver.DeleteFile(fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists(fileName));

            // Exclusão de folder vazio
            _driver.DeleteFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(folderName));
        }

        [TestMethod()]
        public void GetFileInfoTest()
        {
            var folderName = "Folder";
            string fileName = Path.Combine(folderName, "temp.dat");

            // Criação de folder
            _driver.CreateFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName));

            // Cria um arquivo dentro do folter 
            UploadFile(_driver, fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists(fileName));

            var info = _driver.GetFileInfo(fileName);
            Assert.IsNotNull(info);
            Assert.IsTrue(info.Name == "temp.dat");

            // Exclusão de arquivo
            _driver.DeleteFile(fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists(fileName));

            // Exclusão de folder vazio
            _driver.DeleteFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(folderName));
        }

        [TestMethod()]
        public void CopyFileTest()
        {
            var folderName = "Folder";
            string fileName = Path.Combine(folderName, "temp.dat");

            // Criação de folder
            _driver.CreateFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName));

            // Cria um arquivo dentro do folter 
            UploadFile(_driver, fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists(fileName));

            _driver.CopyFile(fileName, "\\", CancellationToken.None);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists(fileName));
            Assert.IsTrue(_driver.FileExists("temp.dat"));

            // Exclusão de arquivo
            _driver.DeleteFile(fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists(fileName));

            _driver.DeleteFile("temp.dat");
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists("temp.dat"));

            // Exclusão de folder vazio
            _driver.DeleteFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(folderName));
        }

        [TestMethod()]
        public void RenameEmptyFolderTest()
        {
            var folderName = "Folder";

            // Criação de folder
            _driver.CreateFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName));

            _driver.RenameFolder("Folder", "NewName");
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists("Folder"));
            Assert.IsTrue(_driver.FolderExists("NewName"));

            // Exclusão de folder vazio
            _driver.DeleteFolder("NewName");
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists("NewName"));
        }

        [TestMethod()]
        public void RenameNotEmptyFolderTest()
        {
            var folderName = "Folder";
            string fileName = Path.Combine(folderName, "temp.dat");

            // Criação de folder
            _driver.CreateFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName));

            // Cria um arquivo dentro do folter 
            UploadFile(_driver, fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists(fileName));

            _driver.RenameFolder("Folder", "NewName");
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists("Folder"));

            Assert.IsTrue(_driver.FolderExists("NewName"));
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists("NewName\\temp.dat"));

            // Exclusão de arquivo
            _driver.DeleteFile("NewName\\temp.dat");
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists("NewName\\temp.dat"));

            // Exclusão de folder vazio
            _driver.DeleteFolder("NewName");
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists("NewName"));
        }

        [TestMethod()]
        public void MoveFolderTest()
        {
            var folderName1 = "Folder1";
            var folderName2 = "Folder2";
            var folderName3 = "Folder3";
            string fileName = Path.Combine(folderName3, "temp.dat");

            // Criação de folder1
            _driver.CreateFolder(folderName1);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName1));
            // Criação de folder2
            _driver.CreateFolder(folderName2);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName2));
            // Criação de folder3
            _driver.CreateFolder(folderName3);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName3));

            // Cria um arquivo dentro do folter 
            UploadFile(_driver, fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists(fileName));

            // Move folder3 para dentro de folter2
            _driver.MoveFolder(folderName3, Path.Combine(folderName2, folderName3));
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(folderName3));
            Assert.IsTrue(_driver.FolderExists(Path.Combine(folderName2, folderName3)));
            // Move folder2 para dentro de folter1
            _driver.MoveFolder(folderName2, Path.Combine(folderName1, folderName2));
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(folderName2));
            Assert.IsTrue(_driver.FolderExists(Path.Combine(folderName1, folderName2)));
            Assert.IsTrue(_driver.FolderExists(Path.Combine(folderName1, Path.Combine(folderName2, folderName3))));

            // Exclusão de arquivo
            _driver.DeleteFile(Path.Combine(folderName1, Path.Combine(folderName2, Path.Combine(folderName3, "temp.dat"))));
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists(fileName));

            // Exclusão de folder vazio
            _driver.DeleteFolder(Path.Combine(folderName1, Path.Combine(folderName2, folderName3)));
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(Path.Combine(folderName1, Path.Combine(folderName2, folderName3))));
            _driver.DeleteFolder(Path.Combine(folderName1, folderName2));
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(Path.Combine(folderName1, folderName2)));
            _driver.DeleteFolder(folderName1);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(folderName1));
        }

        [TestMethod()]
        public void MoveFileTest()
        {
            var folderName = "Folder";
            string fileName = Path.Combine(folderName, "temp.dat");

            // Criação de folder
            _driver.CreateFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName));

            // Cria um arquivo dentro do folter 
            UploadFile(_driver, fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists(fileName));

            _driver.MoveFile(fileName, "\\", CancellationToken.None).GetAwaiter().GetResult();
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists(fileName));
            Assert.IsTrue(_driver.FileExists("temp.dat"));

            // Exclusão de arquivo
            _driver.DeleteFile(fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists(fileName));
            _driver.DeleteFile("temp.dat");
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists("temp.dat"));

            // Exclusão de folder vazio
            _driver.DeleteFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(folderName));
        }

        [TestMethod()]
        public void DownloadFileTest()
        {
            var folderName = "Folder";
            string fileName = Path.Combine(folderName, "temp.dat");

            // Criação de folder
            _driver.CreateFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FolderExists(folderName));

            // Cria um arquivo dentro do folter 
            UploadFile(_driver, fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(_driver.FileExists(fileName));

            var buffer = _driver.DownloadFile(fileName, CancellationToken.None).GetAwaiter().GetResult();
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsTrue(buffer.Length == 1);
            Assert.IsTrue(buffer[0] == 57);

            // Exclusão de arquivo
            _driver.DeleteFile(fileName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FileExists(fileName));

            // Exclusão de folder vazio
            _driver.DeleteFolder(folderName);
            Task.Delay(100).GetAwaiter().GetResult();
            Assert.IsFalse(_driver.FolderExists(folderName));
        }
    }
}
