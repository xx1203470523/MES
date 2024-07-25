using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 物料维护表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public partial class ProcMaterialEntity : BaseEntity
    {
        /// <summary>
        /// 是否唯一码管理
        /// </summary>
        public TrueOrFalseEnum? IsUniqueCode { get; set; }

        /// <summary>
        /// 是否需要IQC检验
        /// </summary>
        public TrueOrFalseEnum? IsIQCInspection { get; set; }

        /// <summary>
        /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
        /// </summary>
        public IQCInspectionTypeEnum? InspectionType { get; set; }

    }
}