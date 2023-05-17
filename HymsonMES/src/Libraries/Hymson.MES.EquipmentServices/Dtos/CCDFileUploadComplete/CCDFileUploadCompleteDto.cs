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
        public string[] SFCs { get; set; } = Array.Empty<string>();

    }
}
