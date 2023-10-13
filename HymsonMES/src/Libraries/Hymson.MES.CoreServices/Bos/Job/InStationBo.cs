using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class InStationRequestBo
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
    public class InStationResponseBo
    {
        /// <summary>
        /// 是否首工序
        /// </summary>
        public bool IsFirstProcedure { get; set; }

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
    public class InStationResponseSummaryBo
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
        /// 
        /// </summary>
        public List<UpdateQtyByWorkOrderIdCommand> UpdateQtyCommands { get; set; } = new();

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
