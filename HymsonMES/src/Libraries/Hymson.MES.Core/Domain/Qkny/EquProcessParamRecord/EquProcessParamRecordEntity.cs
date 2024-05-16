using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquProcessParamRecord
{
    /// <summary>
    /// 数据实体（过程参数记录表）   
    /// equ_process_param_record
    /// @author Yxx
    /// @date 2024-03-11 04:44:02
    /// </summary>
    public class EquProcessParamRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Location { get; set; }

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
