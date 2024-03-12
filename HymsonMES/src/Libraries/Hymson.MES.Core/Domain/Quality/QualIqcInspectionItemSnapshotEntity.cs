using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（IQC检验项目被）   
    /// qual_iqc_inspection_item_snapshot
    /// @author Czhipu
    /// @date 2024-03-06 02:20:42
    /// </summary>
    public class QualIqcInspectionItemSnapshotEntity : BaseEntity
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// proc_material id  物料Id
        /// </summary>
        public long? MaterialId { get; set; }

       /// <summary>
        /// wh_supplier id 供应商id
        /// </summary>
        public long? SupplierId { get; set; }

       /// <summary>
        /// 状态 0、已禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
