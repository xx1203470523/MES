using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;

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
        /// 产品条码列表
        /// </summary>
        public string[]? Sfcs { get; set; }

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
    public class ProductBadRecordResponseBo: JobResultBo
    {

        public bool IsScrapCode { get; set; }
        public List<ManuProductBadRecordEntity> ManuProductBadRecords { get; set; }
        public List<ManuSfcStepEntity> SfcStepList { get; set; }
        public List<ManuSfcProduceBusinessEntity> ManuSfcProduceList { get; set; }
        public  ManuSfcUpdateRouteCommand UpdateRouteCommand { get; set; }
        public ManuSfcUpdateCommand UpdateCommand { get; set; }
        public UpdateIsScrapCommand IsScrapCommand { get; set; }
    }
}
