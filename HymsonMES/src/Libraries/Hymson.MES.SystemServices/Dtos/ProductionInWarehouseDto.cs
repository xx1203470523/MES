using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 生产入库单
    /// </summary>
    public record PackProductionInWarehouseDto : BaseEntityDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        public string InboundOrder { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WareHouseCode { get; set; }

        ///// <summary>
        ///// 入库的箱条码信息
        ///// </summary>
        //public List<InBoxDto> InBoxs { get; set; }

        /// <summary>
        /// 包装箱号数组
        /// </summary>
        public string[] BoxBarCodes { get; set; }
    }

    /// <summary>
    /// 生产入库单
    /// </summary>
    public record ProductionInWarehouseDto : BaseEntityDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        public string InboundOrder { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WareHouseCode { get; set; }

        /// <summary>
        /// 入库的电芯条码
        /// </summary>
        public string[] BarCodes { get; set; }
    }

    public class InBoxDto
    {
        /// <summary>
        /// 箱条码
        /// </summary>
        public string BoxBarCode { get; set; }

        /// <summary>
        /// 箱数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 包装类型1:载具 ，2：包装
        /// </summary>
        public PackagingWarehouTypeEnum Type { get; set; }
    }

    public class InBoxMaterialDto
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
        ///  条码数量
        /// </summary>
        public decimal MaterialBarCodeQty { get; set; }
    }
}
