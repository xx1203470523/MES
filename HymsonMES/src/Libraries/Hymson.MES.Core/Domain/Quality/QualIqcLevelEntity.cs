using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（IQC检验水平）   
    /// qual_iqc_level
    /// @author Czhipu
    /// @date 2024-02-02 02:04:09
    /// </summary>
    public class QualIqcLevelEntity : BaseEntity
    {
        /// <summary>
        /// proc_material id  物料Id
        /// </summary>
        public long? MaterialId { get; set; }

       /// <summary>
        /// wh_supplier id 供应商id
        /// </summary>
        public long? SupplierId { get; set; }

        /// <summary>
        /// 设置类型 1、通用 2、物料
        /// </summary>
        public QCMaterialTypeEnum Type { get; set; }

        /// <summary>
        /// 检验水平 I, II, III, IV, V, VI, VII
        /// </summary>
        public InspectionLevelEnum Level { get; set; }

        /// <summary>
        /// 状态 0、已禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 整体接收水准
        /// </summary>
        public int AcceptanceLevel { get; set; }
        
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
