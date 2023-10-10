using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;

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
    public class InStationResponseSummaryBo
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity> ManuSfcProduceEntities { get; set; } = new List<ManuSfcProduceEntity>();

        /// <summary>
        /// 
        /// </summary>
        public UpdateQtyCommand UpdateQtyCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public MultiUpdateProduceSFCCommand MultiUpdateProduceSFCCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcStepEntity> SFCStepEntities { get; set; } = new List<ManuSfcStepEntity>();

        /// <summary>
        /// 更新条码表
        /// </summary>
        public IEnumerable<InStationManuSfcByIdCommand> InStationManuSfcByIdCommands { get; set; } = new List<InStationManuSfcByIdCommand>();

        /// <summary>  
        /// 更新在制品表
        /// </summary>
        public IEnumerable<MultiUpdateProduceInStationSFCCommand> MultiUpdateProduceInStationSFCCommands { get; set; } = new List<MultiUpdateProduceInStationSFCCommand>();
    }
}
