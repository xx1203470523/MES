using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Services.Dtos.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture.WhMaterialPicking
{
    /// <summary>
    /// 
    /// </summary>
    public class PickMaterialDto
    {
        /// <summary>
        /// 派工单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 退仓类型1：实仓，2：虚仓
        /// </summary>
        public ManuReturnTypeEnum Type { get; set; }
        /// <summary>
        /// 领料数量
        /// </summary>
        public List<PickBomDetail> Details { get; set; }
    }

    public class PickBomDetail
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
