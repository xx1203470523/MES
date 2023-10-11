using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 半成品完工
    /// </summary>
    public class SmiFinishedRequestBo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 载具条码
        /// </summary>
        public string? VehicleCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SmiFinisheResponseSummaryBo
    {
        /// <summary>
        /// 条码信息
        /// </summary>
        public List<ManuSfcEntity> SFCEntities { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public List<ManuSfcStepEntity> SFCStepEntities { get; set; } = new();

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
        public List<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new();

    }
}
