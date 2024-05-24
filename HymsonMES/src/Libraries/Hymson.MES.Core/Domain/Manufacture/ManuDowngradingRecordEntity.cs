using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 降级品录入记录，数据实体对象   
    /// manu_downgrading_record
    /// @author Karl
    /// @date 2023-08-10 10:15:49
    /// </summary>
    public class ManuDowngradingRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 步骤id
        /// </summary>
        public long SFCStepId { get; set; }

        /// <summary>
        /// 品级
        /// </summary>
        public string Grade { get; set; }

       /// <summary>
        /// 是否取消;0 否 1是
        /// </summary>
        public ManuDowngradingRecordTypeEnum? IsCancellation { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; } = 0;

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 条码信息ID
        /// </summary>
        public long? SfcInfoId { get; set; }
    }
}
