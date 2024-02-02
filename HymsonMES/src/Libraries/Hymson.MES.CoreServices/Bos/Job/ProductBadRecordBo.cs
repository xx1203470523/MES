using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// Job请求实体
    /// </summary>
    public class ProductBadRecordRequestBo : JobBaseBo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long[]? UnqualifiedIds { get; set; }

        /// <summary>
        /// 不合格工艺路线id
        /// </summary>
        public long? BadProcessRouteId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// Job返回实体
    /// </summary>
    public class ProductBadRecordResponseBo
    {
        public bool IsScrapCode { get; set; }
        public List<ManuProductBadRecordEntity> ManuProductBadRecords { get; set; } = new List<ManuProductBadRecordEntity>();
        public List<ManuSfcStepEntity> SfcStepList { get; set; } = new List<ManuSfcStepEntity>();
        public List<ManuSfcProduceBusinessEntity> ManuSfcProduceList { get; set; } = new List<ManuSfcProduceBusinessEntity>();
        public ManuSfcUpdateRouteCommand UpdateRouteCommand { get; set; } = new ManuSfcUpdateRouteCommand();
        public ManuSfcUpdateCommand UpdateCommand { get; set; } = new ManuSfcUpdateCommand();
        public UpdateIsScrapCommand IsScrapCommand { get; set; } = new UpdateIsScrapCommand();
    }
}
