﻿using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
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
        /// 条码（首个）
        /// </summary>
        public string FirstSFC { get; set; } = "";

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
        public IEnumerable<InStationManuSfcByIdCommand> InStationManuSfcByIdCommands { get; set; }= new List<InStationManuSfcByIdCommand>();

        /// <summary>  
        /// 更新在制品表
        /// </summary>
        public IEnumerable<MultiUpdateProduceInStationSFCCommand> MultiUpdateProduceInStationSFCCommands { get; set; } = new List<MultiUpdateProduceInStationSFCCommand>();
    }
}
