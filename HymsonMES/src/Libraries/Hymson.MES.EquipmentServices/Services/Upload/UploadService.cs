using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Uploads;
using Hymson.Minio;
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
