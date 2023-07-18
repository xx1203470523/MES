using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class OutStationRequestBo : JobBaseBo
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
    public class OutStationResponseBo
    {
        /// <summary>
        /// 是否完工
        /// </summary>
        public bool IsCompleted { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public List<ManuSfcProduceEntity> SFCProduceEntities { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UpdateQtyByIdCommand> UpdateQtyByIdCommands { get; set; } = new List<UpdateQtyByIdCommand>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new List<ManuSfcCirculationEntity>();

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
        public UpdateQtyCommand UpdateQtyCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public DeleteSfcProduceBusinesssBySfcInfoIdsCommand DeleteSfcProduceBusinesssBySfcInfoIdsCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public MultiSFCUpdateStatusCommand MultiSFCUpdateStatusCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public ProcessRouteTypeEnum ProcessRouteType { get; set; }

        /// <summary>
        /// 下一工序编码
        /// </summary>
        public string NextProcedureCode { get; set; } = "";

    }
}
