using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class RepairStartRequestBo : JobBaseBo
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

    }

    /// <summary>
    /// 
    /// </summary>
    public class RepairStartResponseBo
    {
        /// <summary>
        /// 在制品信息
        /// </summary>
        public ManuSfcProduceEntity SFCProduceEntitiy { get; set; }

        /// <summary>
        /// 步骤
        /// </summary>
        public ManuSfcStepEntity SFCStepEntity { get; set; }

        /// <summary>
        /// 更新条码表
        /// </summary>
        public InStationManuSfcByIdCommand InStationManuSfcByIdCommand { get; set; }

        /// <summary>  
        /// 更新在制品表
        /// </summary>
        public UpdateProduceInStationSFCCommand UpdateProduceInStationSFCCommand { get; set; }


    }

    /// <summary>
    /// 
    /// </summary>
    public class RepairStartResponseSummaryBo : CommonResponseBo
    {
        /// <summary>
        /// 在制品信息
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity>? SFCProduceEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcStepEntity>? SFCStepEntities { get; set; }

        /// <summary>
        /// 更新条码表
        /// </summary>
        public IEnumerable<InStationManuSfcByIdCommand> InStationManuSfcByIdCommands { get; set; } = new List<InStationManuSfcByIdCommand>();

        /// <summary>  
        /// 更新在制品表
        /// </summary>
        public IEnumerable<UpdateProduceInStationSFCCommand> UpdateProduceInStationSFCCommands { get; set; } = new List<UpdateProduceInStationSFCCommand>();
    }

}
