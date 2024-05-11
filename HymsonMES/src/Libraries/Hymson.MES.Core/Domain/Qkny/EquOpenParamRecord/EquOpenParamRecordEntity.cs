using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquOpenParamRecord
{
    /// <summary>
    /// 数据实体（开机参数记录表）   
    /// equ_open_param_record
    /// @author User
    /// @date 2024-05-01 09:23:00
    /// </summary>
    public class EquOpenParamRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

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
        /// 批次Id
        /// </summary>
        public long? BatchId { get; set; }

        /// <summary>
        /// 配方Id
        /// </summary>
        public long RecipeId { get; set; }

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
