using Hymson.MES.Core.Domain.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Job
{
    public class GenerateBarcodeJobResponseBo
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
        public string UserName { get; set; } = "";

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; } = "";

        /// <summary>
        /// 是否产品一致
        /// </summary>
        public bool IsProductSame { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcEntity> ManuSfcList { get; set; } = new List<ManuSfcEntity>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcInfoEntity> ManuSfcInfoList { get; set; } = new List<ManuSfcInfoEntity>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity> ManuSfcProduceList { get; set; } = new List<ManuSfcProduceEntity>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcStepEntity> ManuSfcStepList { get; set; } = new List<ManuSfcStepEntity>();
    }
}
