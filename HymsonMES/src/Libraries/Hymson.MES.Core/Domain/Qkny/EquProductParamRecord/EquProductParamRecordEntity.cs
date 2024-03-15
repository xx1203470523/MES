using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquProductParamRecord
{
    /// <summary>
    /// 数据实体（产品参数记录表）   
    /// equ_product_param_record
    /// @author Yxx
    /// @date 2024-03-13 04:43:35
    /// </summary>
    public class EquProductParamRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 参数Id
        /// </summary>
        public long? ParamId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
