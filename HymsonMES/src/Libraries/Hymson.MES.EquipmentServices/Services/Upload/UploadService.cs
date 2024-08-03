using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.EquipmentServices.Uploads;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.EquipmentServices.Upload
{
    /// <summary>
    /// 上传服务 https://learn.microsoft.com/zh-cn/aspnet/core/mvc/models/file-uploads?view=aspnetcore-6.0
    /// </summary>
    public class UploadService : IUploadService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IMinioService _minioService;
        private readonly string[] _permittedPicExtensions = { ".jpg", ".png", ".gif", ".jpeg", ".bmp" };
        private readonly string[] _permittedFileExtensions = { ".txt", ".pdf", ".xls", ".xlsx", ".wma", ".frx", ".apk", ".jpg", ".png", ".gif", ".jpeg", ".bmp", ".csv", ".docx", ".doc", ".ppt", ".pptx" };

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="minioService"></param>
        public UploadService(IMinioService minioService)
        {
            _minioService = minioService;
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
        /// 文件上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UploadResultDto> UploadFileAsync(UploadFileRequestDto dto)
        {
            await Task.CompletedTask;
            return default;
            /*
            // 文件列表
            var fileList = dto.FormCollection.Files;
            if (fileList == null || !fileList.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES45290));

            List<CcdFileUploadCompleteRecordSaveDto> saveDtoList = new();
            foreach (var file in fileList)
            {
                var fileDir = $"{dto.ProcedureCode}/{HymsonClock.Now():yyyyMMdd}/{equResModel.EquipmentCode}/{dto.Sfc}";

                // 上传
                using var stream = file.OpenReadStream();
                var uploadResult = await _minioService.PutObjectAsync(file.FileName, stream, file.ContentType, fileDir);

                saveDtoList.Add(new CcdFileUploadCompleteRecordSaveDto
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = equResModel.SiteId,
                    EquipmentId = equResModel.EquipmentId,
                    Sfc = dto.Sfc,
                    SfcIsPassed = dto.Passed,
                    Uri = uploadResult.AbsoluteUrl,
                    UriIsPassed = dto.Passed,
                    CreatedBy = equResModel.EquipmentCode,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = equResModel.EquipmentCode,
                    UpdatedOn = HymsonClock.Now()
                });
            }

            //保存
            await _ccdFileUploadCompleteRecordService.AddMultAsync(saveDtoList);
            */
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
