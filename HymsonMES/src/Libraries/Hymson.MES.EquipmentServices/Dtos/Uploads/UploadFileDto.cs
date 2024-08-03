using Microsoft.AspNetCore.Http;

namespace Hymson.MES.EquipmentServices.Uploads
{
    /// <summary>
    /// 设备文件上传DTO
    /// </summary>
    public record UploadFileRequestDto
    {
        /// <summary>
        /// 类型 1:CCD 2:X-RAY 3:其它
        /// </summary>
        public int Type { get; set; } = 1;

        /// <summary>
        /// 工位编码
        /// </summary>
        public string PostionCode { get; set; } = "";

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; } = "";

        /// <summary>
        /// 图片位置或者序号
        /// </summary>
        public string Location { get; set; } = "";

        /// <summary>
        /// 采集完成时间
        /// </summary>
        public DateTime CollectionTime { get; set; }

        /// <summary>
        /// 文件集合
        /// </summary>
        public IFormCollection FormCollection { get; set; }

    }
}
