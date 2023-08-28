using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 半成品完工
    /// </summary>
    public class SmiFinishedRequestBo : JobBaseBo
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class SmiFinisheResponseBo
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new List<ManuSfcCirculationEntity>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcStepEntity> SFCStepEntities { get; set; } = new List<ManuSfcStepEntity>();

        /// <summary>
        /// 
        /// </summary>
        public List<WhMaterialInventoryEntity> WhMaterialInventoryEntities { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public List<WhMaterialStandingbookEntity> WhMaterialStandingbookEntities { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public DeletePhysicalByProduceIdsCommand DeletePhysicalByProduceIdsCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public DeleteSfcProduceBusinesssBySfcInfoIdsCommand DeleteSfcProduceBusinesssBySfcInfoIdsCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public MultiSFCUpdateStatusCommand MultiSFCUpdateStatusCommand { get; set; } = new();

    }
}
