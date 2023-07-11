using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Job
{
    public class BarcodeSfcReceiveBo : JobBaseBo
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

    public class BomMaterial
    {
        /// <summary>
        /// 物料
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }
    }
    public class BarcodeSfcReceiveResponseBo
    {

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

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
        public string UserName { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcEntity> ManuSfcList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcInfoEntity> ManuSfcInfoList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcInfoEntity> UpdateManuSfcInfoList { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity> ManuSfcProduceList { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcStepEntity> ManuSfcStepList { get; set; } 
    }
}
