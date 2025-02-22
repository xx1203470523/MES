﻿using Hymson.MES.EquipmentServices.Uploads;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.EquipmentServices.Upload
{
    /// <summary>
    /// 上传服务
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<UploadResultDto> UploadPicAsync(IFormFile formFile);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<UploadResultDto> UploadFileAsync(IFormFile formFile);

        /// <summary>
        /// 多上传文件（带信息）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IEnumerable<UploadResultDto>> UploadFileAsync(UploadFileRequestDto dto);

    }
}
