using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 退料单
    /// </summary>
    public record ProductionReturnMaterialDto : BaseEntityDto
    {
        /// <summary>
        /// ERP退料单号
        /// </summary>
        public string ERPReturnOrder { get; set; }

        public List<ReturnMaterialDto> ReturnMaterials { get; set; }
    }

    public class ReturnMaterialDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 退料数量
        /// </summary>
        public decimal ReturnQty { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WareHouseCode { get; set; }
    }
}
