using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse
{

    public class WhMaterialInventoryPageListView : WhMaterialInventoryEntity
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 物料单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        ///// <summary>
        ///// 物料条码
        ///// </summary>
        //public string MaterialBarCode { get; set; }

        ///// <summary>
        ///// 批次
        ///// </summary>
        //public string Batch { get; set; }

        ///// <summary>
        ///// 数量（剩余）
        ///// </summary>
        //public decimal QuantityResidue { get; set; }

        ///// <summary>
        ///// 状态;待使用/使用中/锁定
        ///// </summary>
        //public int Status { get; set; }

        ///// <summary>
        ///// 有效期/到期日
        ///// </summary>
        //public DateTime? DueDate { get; set; }

        ///// <summary>
        ///// 来源/目标;手动录入/WMS/上料点编号
        ///// </summary>
        //public int Source { get; set; }

        ///// <summary>
        ///// 站点Id
        ///// </summary>
        //public long SiteId { get; set; }

        ///// <summary>
        ///// 是否删除 
        ///// </summary>
        //public long IsDeleted { get; set; }

    }


    /// <summary>
    /// 物料信息
    /// </summary>
    public record ProcMaterialInfoView : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料批次大小
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 物料版本
        /// </summary>

        public string Version { get; set; }

        /// <summary>
        /// 单位
        /// </summary>

        public string Unit { get; set; }
    }

    /// <summary>
    /// 供应商信息
    /// </summary>
    public record WhSupplierInfoView : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }
    }


}
