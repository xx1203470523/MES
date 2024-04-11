using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class StopRequestBo : JobBaseBo
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
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class StopResponseBo
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ManuSfcProduceEntity> SFCProduceEntities { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public List<ManuSfcStepEntity> SFCStepEntities { get; set; } = new();
        /// <summary>
        /// 
        /// </summary>
        public List<InStationManuSfcByIdCommand> InStationManuSfcByIdCommands { get; set; } = new();
    }
}
