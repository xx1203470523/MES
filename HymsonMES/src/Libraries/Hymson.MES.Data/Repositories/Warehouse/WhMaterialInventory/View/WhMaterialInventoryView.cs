using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 
    /// </summary>
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
        public decimal? Batch { get; set; } = 0;

        /// <summary>
        /// 标包
        /// </summary>
        public int PackageNum { get; set; }
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
