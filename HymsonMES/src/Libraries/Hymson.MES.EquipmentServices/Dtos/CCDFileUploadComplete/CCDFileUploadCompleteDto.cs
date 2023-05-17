using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.CCDFileUploadComplete
{
    /// <summary>
    /// CCD文件上传完成
    /// </summary>
    public record CCDFileUploadCompleteDto : BaseDto
    {
        /// <summary>
        /// 条码（多个）
        /// </summary>
        public IEnumerable<CCDFileUploadCompleteDetail> SFCs { get; set; } = new List<CCDFileUploadCompleteDetail>();

    }

    /// <summary>
    /// CCD文件上传完成明细
    /// </summary>
    public class CCDFileUploadCompleteDetail
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 是否合格
        /// </summary>
        public int Passed { get; set; } = 1;

        /// <summary>
        /// 上传完成路径
        /// </summary>
        public string URI { get; set; } = string.Empty;

        /// <summary>
        /// 采集完成时间
        /// </summary>
        public string Timestamp { get; set; } = string.Empty;

    }
}
