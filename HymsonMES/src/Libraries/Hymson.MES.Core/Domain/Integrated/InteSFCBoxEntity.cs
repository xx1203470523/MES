using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Integrated
{
    public class InteSFCBoxEntity: BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 电芯条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 箱码
        /// </summary>
        public string BoxCode { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// 状态;1、开启 2、恢复
        /// </summary>
        public SFCBoxEnum? Status { get; set; }

        /// <summary>
        /// 传输时间
        /// </summary>
        public DateTime? Localtime { get; set; } 

        /// <summary>
        /// OCVB
        /// </summary>
        public decimal? OCVB { get; set; }

        /// <summary>
        /// OCVB数采时间
        /// </summary>
        public DateTime? OcvbDate { get; set; }

        /// <summary>
        /// 二次注液注液后重量
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// DC容量
        /// </summary>
        public decimal? DC { get; set; }

        /// <summary>
        /// DC容量数采时间
        /// </summary>
        public DateTime? DcDate { get; set; }

        /// <summary>
        /// IMPB
        /// </summary>
        public decimal? IMPB { get; set; }

        /// <summary>
        /// 自放电率X
        /// </summary>
        public decimal? SelfDischargeRate { get; set; }

        /// <summary>
        /// 电芯宽度（包膜）C-D区域
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// 电芯正极极柱高度
        /// </summary>
        public decimal? HeightZ { get; set; }

        /// <summary>
        /// 电芯负极极柱高度
        /// </summary>
        public decimal? HeightF { get; set; }

        /// <summary>
        /// 电芯正极肩高
        /// </summary>
        public decimal? ShoulderHeightZ { get; set; }

        /// <summary>
        /// 电芯负极肩高
        /// </summary>
        public decimal? ShoulderHeightF { get; set; }

        /// <summary>
        /// 电芯厚度（包膜）
        /// </summary>
        public decimal? Thickness { get; set; }
    }

    public class InteSFCBoxBatch
    {
        public string BatchNo { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
