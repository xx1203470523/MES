using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.EquipmentServices.Uploads;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Hymson.MES.EquipmentServices.Upload
{
    /// <summary>
    /// 上传服务 https://learn.microsoft.com/zh-cn/aspnet/core/mvc/models/file-uploads?view=aspnetcore-6.0
    /// </summary>
    public class UploadService : IUploadService
    {
        /// <summary>
        /// 仓储接口（设备文件上传）
        /// </summary>
        private readonly IEquFileUploadRepository _equFileUploadRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMinioService _minioService;
        private readonly string[] _permittedPicExtensions = { ".jpg", ".png", ".gif", ".jpeg", ".bmp" };
        private readonly string[] _permittedFileExtensions = { ".txt", ".pdf", ".xls", ".xlsx", ".wma", ".frx", ".apk", ".jpg", ".png", ".gif", ".jpeg", ".bmp", ".csv", ".docx", ".doc", ".ppt", ".pptx" };

        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadService(IMinioService minioService,
            IEquFileUploadRepository equFileUploadRepository)
        {
            _minioService = minioService;
            _equFileUploadRepository = equFileUploadRepository;
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task<UploadResultDto> UploadPicAsync(IFormFile formFile)
        {
            var fileExt = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExt) || !_permittedPicExtensions.Contains(fileExt))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11609)).WithData("Extensions", string.Join(',', _permittedPicExtensions));
            }
            return await UploadFileAsync(formFile, fileExt);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task<UploadResultDto> UploadFileAsync(IFormFile formFile)
        {
            var fileExt = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExt) || !_permittedFileExtensions.Contains(fileExt))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11609)).WithData("Extensions", string.Join(',', _permittedFileExtensions));
            }
            return await UploadFileAsync(formFile, fileExt);
        }

        /// <summary>
        /// 多上传文件（带信息）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UploadResultDto>> UploadFileAsync(UploadFileRequestDto dto)
        {
            // 文件列表
            var fileList = dto.FormCollection.Files;
            if (fileList == null || !fileList.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES11600));

            List<UploadResultDto> uploadResultDtos = new();
            List<EquFileUploadEntity> addList = new List<EquFileUploadEntity>();
            foreach (var file in fileList)
            {
                var fileDir = $"{dto.PostionCode}/{HymsonClock.Now():yyyyMMdd}/{dto.BarCode}";

                // 上传
                using var stream = file.OpenReadStream();
                var uploadResult = await _minioService.PutObjectAsync(file.FileName, stream, file.ContentType, fileDir);

                var fileExt = Path.GetExtension(file.FileName).ToLowerInvariant();
                uploadResultDtos.Add(new UploadResultDto
                {
                    FileExt = fileExt,
                    FileType = file.ContentType,
                    OriginalName = file.FileName,
                    FileSize = file.Length,
                    FileUrl = uploadResult.AbsoluteUrl
                });

                EquFileUploadEntity addModel = new EquFileUploadEntity();
                addModel.Id = IdGenProvider.Instance.CreateId();
                addModel.CreatedOn = HymsonClock.Now();
                addModel.UpdatedOn = addModel.CreatedOn;
                addModel.CreatedBy = "uploadApi";
                addModel.UpdatedBy = addModel.CreatedBy;
                addModel.BarCode = dto.BarCode;
                addModel.PostionCode = dto.PostionCode;
                addModel.Location = dto.Location;
                addModel.CollectionTime = dto.CollectionTime;
                addModel.FileExt = fileExt;
                addModel.FileSize = file.Length;
                addModel.FileUrl = uploadResult.AbsoluteUrl;
                addList.Add(addModel);
            }

            try
            {
                await _equFileUploadRepository.InsertRangeAsync(addList);
            }
            catch(Exception ex)
            {

            }

            return uploadResultDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        private async Task<UploadResultDto> UploadFileAsync(IFormFile formFile, string fileExt)
        {
            var realName = formFile.FileName;
            var uploadResultDto = new UploadResultDto
            {
                FileType = formFile.ContentType,
                FileExt = fileExt,
                OriginalName = realName
            };

            using var stream = formFile.OpenReadStream();
            uploadResultDto.FileSize = formFile.Length;

            var uploadResult = await _minioService.PutObjectAsync(realName, stream, formFile.ContentType);
            uploadResultDto.FileUrl = uploadResult.AbsoluteUrl;

            return uploadResultDto;
        }

    }
}
