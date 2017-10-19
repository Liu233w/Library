using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Models;
using Library.Authorization;
using Library.Configuration;
using Library.Controllers.Dto;
using Library.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class UploadsController : LibraryControllerBase
    {
        private readonly BinaryObjectManager _binaryObjectManager;

        public UploadsController(BinaryObjectManager binaryObjectManager)
        {
            _binaryObjectManager = binaryObjectManager;
        }

        [HttpPost] // 直接将文件post到此路由，默认文件公开 见BinaryObject.cs
        [UnitOfWork]
        public async Task<JsonResult> UploadFiles()
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                try
                {
                    var files = Request.Form.Files;

                    //Check input
                    if (files == null)
                    {
                        throw new UserFriendlyException(L("File_Empty_Error"));
                    }

                    List<UploadFileOutput> filesOutput = new List<UploadFileOutput>();

                    foreach (var file in files)
                    {
                        // if (file.Length > 1048576) //1MB
                        if (file.Length > 104857600) //100MB
                        {
                            throw new UserFriendlyException(L("File_SizeLimit_Error"));
                        }

                        if (file.FileName.Length > 256) //检查文件名长度
                        {
                            throw new UserFriendlyException(L("File_Name_Too_Long"));
                        }

                        var fileObject = await _binaryObjectManager.SaveAsync(file.OpenReadStream(), file.FileName, AbpSession.TenantId);

                        filesOutput.Add(new UploadFileOutput
                        {
                            Id = fileObject.Id,
                            FileName = file.FileName
                        });
                    }

                    return Json(new AjaxResponse(filesOutput));
                }
                catch (UserFriendlyException ex)
                {
                    return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
                }
            }
        }

        // [DisableAuditing]
        [HttpGet] // 下载文件，Guid Id, String FileName
        // [AbpAuthorize]
        [UnitOfWork]
        public async Task<ActionResult> GetFile(GetFileInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                var file = await _binaryObjectManager.GetOrNullAsync(input.Id);
                if (file == null) // 检查文件是否存在
                {
                    throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
                }

                var bytes = await _binaryObjectManager.ReadAsync(file);
                return File(bytes, AllowExt.GetMimeType(file.FileName), input.FileName); // type根据上传的文件名确定，下载的文件名由前端确定
            }
        }
    }
}