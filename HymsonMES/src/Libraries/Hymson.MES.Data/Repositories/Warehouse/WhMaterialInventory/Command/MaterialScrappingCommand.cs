using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    public class MaterialScrappingCommand
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        public long? SiteId { get; set; }

    }

    public class WhMaterialInventoryScrapPagedQuery : PagerInfo
    {

        /// <summary>
        /// 物料条码
        /// </summary>
        public TrueOrFalseEnum? IsCancellation { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

    }
}
