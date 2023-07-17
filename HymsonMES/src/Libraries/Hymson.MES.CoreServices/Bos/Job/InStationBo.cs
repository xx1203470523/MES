using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class InStationRequestBo : JobBaseBo
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
    public class InStationResponseBo : JobResultBo
    {
        /// <summary>
        /// 是否首工序
        /// </summary>
        public bool IsFirstProcedure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity> SFCProduceEntities { get; set; } = new List<ManuSfcProduceEntity>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UpdateWorkOrderRealTimeCommand> UpdateWorkOrderRealTimeCommands = new List<UpdateWorkOrderRealTimeCommand>();

        /// <summary>
        /// 
        /// </summary>
        public UpdateQtyCommand UpdateQtyCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcStepEntity> SFCStepEntities { get; set; } = new List<ManuSfcStepEntity>();

        /// <summary>
        /// 
        /// </summary>
        public MultiSfcUpdateIsUsedCommand MultiSfcUpdateIsUsedCommand { get; set; } = new();

    }

}
