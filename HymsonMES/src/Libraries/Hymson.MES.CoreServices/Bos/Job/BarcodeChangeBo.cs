using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Job
{
    public class BarcodeChangeBo
    {
        
       
        /// <summary>
        /// 工单产品Id 用于校验条码掩码
        /// </summary>
        public PlanWorkOrderEntity WO { get; set; }
        /// <summary>
        /// 多条码转换
        /// </summary>
        public IEnumerable<BarcodeChangeBoItem> Items { get; set; }


        public class BarcodeChangeBoItem
        {
            /// <summary>
            /// 转换后条码
            /// </summary>
            public string TargetSFC { get; set; }
            /// <summary>
            /// 被转换条码,如果条码为空，执行条码接收逻辑
            /// </summary>
            public string? SourceSFC { get; set; }
            
            /// <summary>
            ///条码流转类型
            /// </summary>
            public SfcCirculationTypeEnum CirculationType { get; set; }
            /// <summary>
            /// 条码步骤表:步骤类型
            /// </summary>
            public ManuSfcStepTypeEnum SourceStepType { get; set; }
            /// <summary>
            /// 新条码状态
            /// </summary>
            public SfcStatusEnum Status { get; set; } = SfcStatusEnum.lineUp;
        }


    }
    public class BarcodeChangeResponse
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }
        public string WorkCode { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQuantity { get; set; }

        /// <summary>
        /// 下达数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";
        public IEnumerable<ManuSfcEntity> manusfcs { get; set; }
        public IEnumerable<ManuSfcInfoEntity> sfcinfos { get; set; }
        public IEnumerable<ManuSfcProduceEntity> sfcproduces { get; set; }
        public IEnumerable<ManuSfcCirculationEntity>  manuSfcCirculationEntitys { get; set; }
        public IEnumerable<ManuSfcStepEntity> manuSfcStepEntities { get; set; }
        public IEnumerable<PhysicalDeleteSFCProduceByIdsCommand> PhysicalDeleteSFCProduceByIdsCommands { get; set; }
        public IEnumerable<MultiSFCUpdateStatusCommand> MultiSFCUpdateStatusCommands { get; set; }
    }
    public class BarcodeMergeResponse
    {
        
        public IEnumerable<ManuSfcEntity> manusfcs { get; set; }
        public IEnumerable<ManuSfcInfoEntity> sfcinfos { get; set; }
        public IEnumerable<ManuSfcProduceEntity> sfcproduces { get; set; }
        public IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys { get; set; }
        public IEnumerable<ManuSfcStepEntity> manuSfcStepEntities { get; set; }
      
    }
}
