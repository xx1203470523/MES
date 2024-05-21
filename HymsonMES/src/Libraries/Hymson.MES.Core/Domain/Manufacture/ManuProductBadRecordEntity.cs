using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 产品不良录入，数据实体对象   
    /// manu_product_bad_record
    /// @author zhaoqing
    /// @date 2023-03-27 03:49:17
    /// </summary>
    public class ManuProductBadRecordEntity : BaseEntity
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 发现不良工序Id
        /// </summary>
        public long FoundBadOperationId { get; set; }

        /// <summary>
        /// 发现不良资源
        /// </summary>
        public long? FoundBadResourceId { get; set; }

        /// <summary>
        /// 流出不良工序
        /// </summary>
        public long OutflowOperationId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 条码id
        /// </summary>
        public long? SfcInfoId { get; set; }

        /// <summary>
        /// 条码步骤表
        /// </summary>
        public long? SfcStepId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum Status { get; set; }

        /// <summary>
        /// 不良来源;·1、设备复投不良  2、人工录入不良
        /// </summary>
        public ProductBadRecordSourceEnum? Source { get; set; }

        /// <summary>
        /// 处置结果
        /// </summary>
        public ProductBadDisposalResultEnum? DisposalResult { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 关闭人
        /// </summary>
        public string? CloseBy { get; set; }

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseOn { get; set; }

        /// <summary>
        /// 关闭步骤表id
        /// </summary>
        public long? CloseSfcStepId { get; set; }

        /// <summary>
        /// 复判人
        /// </summary>
        public string? ReJudgmentBy { get; set; }

        /// <summary>
        /// 复判时间
        /// </summary>
        public DateTime? ReJudgmentOn { get; set; }

        /// <summary>
        /// 复判结果
        /// </summary>
        public ProductBadDisposalResultEnum? ReJudgmentResult { get; set; }

        /// <summary>
        /// 复判步骤表id
        /// </summary>
        public long? ReJudgmentSfcStepId { get; set; }

        /// <summary>
        /// 拦截工序
        /// </summary>
        public long? InterceptOperationId { get; set; }
    }
}
