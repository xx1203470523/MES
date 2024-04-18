using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// CCD文件上传完成
    /// </summary>
    public record CCDFileUploadCompleteDto : QknyBaseDto
    {
        /// <summary>
        /// 条码文件信息
        /// </summary>
        public List<CCDFileSfcDto> SfcList { get; set; } = new List<CCDFileSfcDto>();
    }

    /// <summary>
    /// CCD文件条码信息
    /// </summary>
    public record CCDFileSfcDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 是否合格
        /// </summary>
        public int Passed { get; set; }

        /// <summary>
        /// 上传完成路径
        /// </summary>
        public List<CCDSfcFileDto> UriList { get; set; } = new List<CCDSfcFileDto>();

        /// <summary>
        /// 采集完成时间
        /// </summary>
        public DateTime CollectionTime { get; set; }
    }

    /// <summary>
    /// CCD单个文件信息
    /// </summary>
    public record CCDSfcFileDto
    {
        /// <summary>
        /// 单个文件路径
        /// </summary>
        public string Uri { get; set; } = "";

        /// <summary>
        /// 是否合格
        /// </summary>
        public int Passed { get; set; }
    }
}
