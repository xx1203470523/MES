using Microsoft.AspNetCore.Http;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Common
{
    /// <summary>
    /// 设备文件上传DTO
    /// </summary>
    public record EquFileUploadDto : QknyBaseDto
    {
        /// <summary>
        /// 类型 1:CCD 2:X-RAY 3:其它
        /// </summary>
        public int Type { get; set; } = 1;

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 是否合格
        /// </summary>
        public int Passed { get; set; }

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
