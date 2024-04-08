using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuProductBadRecordCommand
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum? Status { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum? CurrentStatus { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 关闭人
        /// </summary>
        public string? CloseBy { get; set; }

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseOn { get; set; }

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
        /// 复判备注
        /// </summary>
        public string? ReJudgmentRemark { get; set; }

        /// <summary>
        /// 复判步骤表id
        /// </summary>
        public long? ReJudgmentSfcStepId { get; set; }
    }

    public class ManuProductBadRecordUpdateCommand
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; } 

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum? Status { get; set; }

        /// <summary>
        /// 处置结果
        /// </summary>
        public ProductBadDisposalResultEnum? DisposalResult { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
