using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 请求（让步接收）
    /// </summary>
    public class CompromiseRequestBo
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
        /// 工序
        /// </summary>
        public IEnumerable<ProcProcedureEntity> ProcedureEntities { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public IEnumerable<QualUnqualifiedCodeEntity> UnqualifiedCodeEntities { get; set; }

        /// <summary>
        /// 不合格记录（分组）
        /// </summary>
        public Dictionary<string, IGrouping<string, ManuProductBadRecordEntity>> BadRecordEntitiesDict { get; set; }
    }

}
