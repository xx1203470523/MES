using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;

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
        public bool IsCompleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity> SFCProduceEntities { get; set; } = new List<ManuSfcProduceEntity>();

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
        public IEnumerable<ManuSfcStepEntity> SFCStepEntities { get; set; } = new List<ManuSfcStepEntity>();

    }
}
