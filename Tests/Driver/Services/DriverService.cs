using Contracts.Entity.Driver;

using Microsoft.AspNetCore.Components.Forms;

using Newtonsoft.Json;

using Shared;

using System.Net;
using System.Net.Http.Headers;

namespace Driver.Services
{
    public class DriverService : HttpAbstractService, IDriverService
    {
        public DriverService(string uri)
            : base(
                uri,
                new HttpClient(new HttpClientHandler
                {
                    // Bypass the SSH certificate
                    ServerCertificateCustomValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => { return true; }
                }
                )
            )
        { /* Nothing more todo */ }

        /// <summary>
        /// Retorna as informações sobre os arquivos e pastas na RAIZ do driver remoto
        /// </summary>
        public async Task<List<Models.FileInfoModel>> GetRootFilesInfo()
        {
            string uri = $"{_requestUri}get/root/files/info";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await SendAsync(requestMessage);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var list = await Task.FromResult(JsonConvert.DeserializeObject<List<Models.FileInfoModel>>(responseBody));
                return list ?? new();
            }

            return new();
        }

        /// <summary>
        /// Retorna as informações sobre os arquivos e pastas de uma determinada prasta do driver remoto
        /// </summary>
        /// <param name="relpath">Pasta no driver remoto</param>
        /// <returns>Arquivos e pastas dentro de relpath</returns>
        public async Task<List<Models.FileInfoModel>> GetFolderFilesInfo(string relpath)
        {
            string uri = $"{_requestUri}get/path/files/info?relpath={relpath}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await SendAsync(requestMessage);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var list = await Task.FromResult(JsonConvert.DeserializeObject<List<Models.FileInfoModel>>(responseBody));
                return list ?? new();
            }

            return new();
        }

        /// <summary>
        /// Cria uma pasta relahenrique VIItiva à RAIZ do driver remoto
        /// </summary>
        /// <param name="folder">Caminho da pasta a ser criada</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CreateFolder(string folder)
        {
            string uri = $"{_requestUri}create/folder?relpath={folder}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var response = await SendAsync(requestMessage);

            return response;
        }

        public async Task<HttpResponseMessage> UploadFile(IBrowserFile file, string relativePath)
        {
            string uri = $"{_requestUri}upload/file";
            HttpResponseMessage response;

            using (var form = new MultipartFormDataContent())
            {
                var buffer = await ReadTemplateFile(file);
                using var fileContent = new ByteArrayContent(buffer);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                // "file" parameter name should be the same as the server side input parameter name
                form.Add(fileContent, "file", relativePath + Path.GetFileName(file.Name));

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                response = await _httpClient.PostAsync(uri, form);
            }

            return response;
        }

        public async Task<FileContentResponse?> DownloadFile(string path)
        {
            string uri = $"{_requestUri}download/file?filename={path}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await SendAsync(requestMessage);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var fileContent = JsonConvert.DeserializeObject<FileContentResponse>(content);

                return fileContent;
            }

            return null!;
        }

        public async Task<HttpResponseMessage> DeleteFile(string path)
        {
            string uri = $"{_requestUri}delete/file/?filename={path}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            var response = await SendAsync(requestMessage);
            return await Task.FromResult(response);
        }

        public async Task<HttpResponseMessage> DeleteFolder(string folder)
        {
            string uri = $"{_requestUri}delete/folder/?path={folder}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            var response = await SendAsync(requestMessage);
            return await Task.FromResult(response);
        }

        public async Task<HttpResponseMessage> GetFileInfo(string file)
        {
            string uri = $"{_requestUri}get/info?relpath={file}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> CopyFile(string source, string target)
        {
            string uri = $"{_requestUri}copy/file?source={source}&target={target}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
            var response = await SendAsync(requestMessage);
            return await Task.FromResult(response);
        }

        public async Task<HttpResponseMessage> Rename(string oldname, string newname)
        {
            string uri = $"{_requestUri}rename/folder?oldname={oldname}&newname={newname}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
            var response = await SendAsync(requestMessage);
            return await Task.FromResult(response);
        }

        public async Task<HttpResponseMessage> MoveFolder(string source, string target)
        {
            string uri = $"{_requestUri}move/folder?source={source}&target={target}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
            var response = await SendAsync(requestMessage);
            return await Task.FromResult(response);
        }

        public async Task<HttpResponseMessage> MoveFileToFolder(string file, string folder)
        {
            string uri = $"{_requestUri}move/file?file={file}&folder={folder}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
            var response = await SendAsync(requestMessage);
            return await Task.FromResult(response);
        }

        private static async Task<byte[]> ReadTemplateFile(IBrowserFile file)
        {
            Stream fileStream = file.OpenReadStream();

            var data = new byte[fileStream.Length];
            var buffer = new byte[32000];
            int offset = 0;
            int count = await fileStream.ReadAsync(buffer);

            do
            {
                Array.Copy(buffer, 0, data, offset, count);
                offset += count;
                count = await fileStream.ReadAsync(buffer);
            }
            while (count > 0);

            fileStream.Close();

            return data;
        }
    }
}
