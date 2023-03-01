using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Application.Handlers.Driver.Query;
using Application.Handlers.Driver.Common;
using Application.Handlers.Driver.Create;
using Application.Handlers.Driver.Update;
using Application.Handlers.Driver.Delete;
using DriverApi.Base;
using Contracts.Entity.Driver;

namespace DriverApi.Controllers
{
    [ApiController]
    [Route("driver")]
    public class DriverController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public DriverController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("get/root/files/info")]
        public async Task<IActionResult> GetRootFiles()
        {
            var query = new GetRootFilesQuery(
                );

            ErrorOr<IEnumerable<FileInfoResult>> result = await _mediator.Send(query);

            return result.Match(
                result => Ok(_mapper.Map<List<FileInfoResponse>>(result)),
                errors => Problem(errors)
                );
        }

        [HttpGet("get/path/files/info")]
        public async Task<IActionResult> GetFolderFiles([FromQuery] string relpath)
        {
            var query = new GetFolderFilesQuery(
                relpath
                );

            ErrorOr<IEnumerable<FileInfoResult>> result = await _mediator.Send(query);

            return result.Match(
                result => Ok(_mapper.Map<List<FileInfoResponse>>(result)),
                errors => Problem(errors)
                );
        }

        [HttpGet("get/info")]
        public async Task<IActionResult> GetFileInfo([FromQuery] string relpath)
        {
            var query = new GetFileInfoQuery(
                relpath
                );

            ErrorOr<FileInfoResult> result = await _mediator.Send(query);

            return result.Match(
                result => Ok(_mapper.Map<FileInfoResponse>(result)),
                errors => Problem(errors)
                );
        }

        [HttpPost("upload/file")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file is null)
                return BadRequest();

            var command = new UploadFileCommand(
                file
                );

            ErrorOr<FileInfoResult> result = await _mediator.Send(command);

            return result.Match(
                result => Ok(_mapper.Map<FileInfoResponse>(result)),
                errors => Problem(errors)
                );
        }

        [HttpPost("create/folder")]
        public async Task<IActionResult> CreateFolder([FromQuery] string relpath)
        {
            var command = new CreateFolderCommand(
                relpath
                );

            ErrorOr<string> result = await _mediator.Send(command);

            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
                );
        }

        [HttpPut("copy/file")]
        public async Task<IActionResult> CopyFile([FromQuery] string source, [FromQuery] string target)
        {
            var command = new CopyFileCommand(
                source,
                target
                );

            ErrorOr<bool> result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
                );
        }

        [HttpPut("rename/folder")]
        public async Task<IActionResult> RenameFolder([FromQuery] string oldname, [FromQuery] string newname)
        {
            var command = new RenameFolderCommand(
                oldname,
                newname
                );

            ErrorOr<bool> result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
                );
        }

        [HttpPut("move/folder")]
        public async Task<IActionResult> MoveFolder([FromQuery] string source, [FromQuery] string target)
        {
            var command = new MoveFolderCommand(
                source,
                target
                );

            ErrorOr<bool> result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
                );
        }

        [HttpPut("move/file")]
        public async Task<IActionResult> MoveFile([FromQuery] string file, [FromQuery] string folder)
        {
            var command = new MoveFileCommand(
                file,
                folder
                );

            ErrorOr<bool> result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
                );
        }

        [HttpGet("download/file")]
        public async Task<IActionResult> DownloadFile([FromQuery] string filename)
        {
            var query = new DownloadFileQuery(
                filename
                );

            ErrorOr<CustomFileContentResult> result = await _mediator.Send(query);

            return result.Match(
                result => Ok(_mapper.Map<FileContentResponse>(result)),
                errors => Problem(errors)
                );
        }

        [HttpDelete("delete/file")]
        public async Task<IActionResult> DeleteFile([FromQuery] string filename)
        {
            var command = new DeleteFileCommand(
                filename
                );

            ErrorOr<bool> result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
                );
        }

        [HttpDelete("delete/folder")]
        public async Task<IActionResult> DeleteFolder([FromQuery] string path)
        {
            var command = new DeleteFolderCommand(
                path
                );

            ErrorOr<bool> result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
                );
        }
    }
}
