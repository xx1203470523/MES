using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 请求（返工）
    /// </summary>
    public class ReworkRequestBo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<ManuSfcEntity> SFCEntities { get; set; }

        /// <summary>
        /// 条码信息
        /// </summary>
        public IEnumerable<ManuSfcInfoEntity> SFCInfoEntities { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public IEnumerable<PlanWorkOrderEntity> WorkOrderEntities { get; set; }

        /// <summary>
        /// 在制品
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity> SFCProduceEntities { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public IEnumerable<ProcMaterialEntity> ProductEntities { get; set; }

        /// <summary>
        /// 不合格记录
        /// </summary>
        public IEnumerable<ManuProductBadRecordEntity> BadRecordEntities { get; set; }
    }

    /// <summary>
    /// 响应（返工）
    /// </summary>
    public class ReworkResponseBo
    {
        /// <summary>
        /// 在制品（新增）
        /// </summary>
        public List<ManuSfcProduceEntity> InsertSFCProduceEntities { get; set; } = new();

        /// <summary>
        /// 在制品（修改）
        /// </summary>
        public List<ManuSfcProduceEntity> UpdateSFCProduceEntities { get; set; } = new();

        /// <summary>
        /// 步骤
        /// </summary>
        public List<ManuSfcStepEntity> SFCStepEntities { get; set; } = new();

        /// <summary>
        /// 不良记录
        /// </summary>
        public List<ManuProductBadRecordEntity> ProductBadRecordEntities { get; set; } = new();

        /// <summary>
        /// NG记录
        /// </summary>
        public List<ManuProductNgRecordEntity> ProductNgRecordEntities { get; set; } = new();

        /// <summary>
        /// 不良记录更新命令
        /// </summary>
        public List<ManuProductBadRecordUpdateCommand> BadRecordUpdateCommands { get; set; } = new();

    }
}
