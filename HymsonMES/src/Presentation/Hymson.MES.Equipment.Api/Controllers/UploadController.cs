using Hymson.MES.EquipmentServices.Upload;
using Hymson.MES.EquipmentServices.Uploads;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 上传
    /// </summary>
    [Route("upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        /// <summary>
        /// 上传服务
        /// </summary>
        private readonly IUploadService _uploadService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uploadService"></param>
        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost, Route("uploadImg")]
        [RequestSizeLimit(104857600)]
        public async Task<UploadResultDto> UploadImg(IFormFile file)
        {
            return await _uploadService.UploadPicAsync(file);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost, Route("uploadFile")]
        [RequestSizeLimit(104857600)]
        public async Task<UploadResultDto> UploadFileAsync(IFormFile file)
        {
            return await _uploadService.UploadFileAsync(file);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("uploadFileWithInfo")]
        [LogDescription("文件上传", BusinessType.OTHER, "Upload", ReceiverTypeEnum.MES)]
        public async Task<UploadResultDto> UploadFileAsync([FromForm] UploadFileRequestDto dto)
        {
            return await _uploadService.UploadFileAsync(dto);
        }

    }
}
