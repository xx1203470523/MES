using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    public record QualMaterialUnqualifiedDataViewDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 物料(编码/版本)
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不良状态;1、打开 2、关闭
        /// </summary>
        public QualMaterialUnqualifiedStatusEnum UnqualifiedStatus { get; set; }

        /// <summary>
        /// 处置结果;1、放行 2、退料
        /// </summary>
        public QualMaterialDisposalResultEnum? DisposalResult { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 处置时间
        /// </summary>
        public DateTime? DisposalTime { get; set; }
    }

    /// <summary>
    /// 车间物料不良记录新增/更新Dto
    /// </summary>
    public record QualMaterialUnqualifiedDataSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 物料库存Id;wh_material_inventory的Id
        /// </summary>
        public long MaterialInventoryId { get; set; }

        /// <summary>
        /// 不良备注
        /// </summary>
        public string UnqualifiedRemark { get; set; } = "";

        /// <summary>
        /// 不合格代码
        /// </summary>
        public IEnumerable<QualMaterialUnqualifiedDataDetailSaveDto> DetailDtos { get; set; }
    }

    public record QualMaterialUnqualifiedDataDetailSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 不合格代码组ID;qual_unqualified_group的Id
        /// </summary>
        public long UnqualifiedGroupId { get; set; }

        /// <summary>
        /// 不合格代码ID;qual_unqualified_code的Id
        /// </summary>
        public long UnqualifiedCodeId { get; set; }
    }

    /// <summary>
    /// 车间物料不良记录新处置Dto
    /// </summary>
    public record QualMaterialUnqualifiedDataDisposalDto : BaseEntityDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 处置结果;1、放行 2、退料
        /// </summary>
        public QualMaterialDisposalResultEnum DisposalResult { get; set; }

        /// <summary>
        /// 处置备注
        /// </summary>
        public string DisposalRemark { get; set; } = "";
    }

    /// <summary>
    /// 车间物料不良记录Dto
    /// </summary>
    public record QualMaterialUnqualifiedDataDto : BaseEntityDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料库存Id;wh_material_inventory的Id
        /// </summary>
        public long MaterialInventoryId { get; set; }

        /// <summary>
        /// 物料组Id
        /// </summary>
        public long MaterialGroupId { get; set; } = 0;

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 物料(编码/版本)
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 不良备注
        /// </summary>
        public string UnqualifiedRemark { get; set; }

        /// <summary>
        /// 处置结果;1、放行 2、退料
        /// </summary>
        public QualMaterialDisposalResultEnum? DisposalResult { get; set; }

        /// <summary>
        /// 处置备注
        /// </summary>
        public string DisposalRemark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 不合格组和不合格代码
        /// </summary>
        public IEnumerable<QualMaterialUnqualifiedDetailDataDto> Details { get; set; }
    }


    public record QualMaterialUnqualifiedDetailDataDto : BaseEntityDto
    {
        /// <summary>
        /// 不合格代码组Id
        /// </summary>
        public long UnqualifiedGroupId { get; set; }

        /// <summary>
        /// 不合格组代码+"/"+不合格组名称
        /// </summary>
        public string UnqualifiedGroupRemark { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedCodeId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码+"/"+不合格代码名称
        /// </summary>
        public string UnqualifiedCodeRemark { get; set; }
    }

    /// <summary>
    /// 车间物料不良记录分页Dto
    /// </summary>
    public class QualMaterialUnqualifiedDataPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string? MaterialBarCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 不合格代码组ID
        /// </summary>
        public long? UnqualifiedGroupId { get; set; }

        /// <summary>
        /// 不合格代码ID
        /// </summary>
        public long? UnqualifiedCodeId { get; set; }

        ///// <summary>
        ///// 不合格组
        ///// </summary>
        //public string? UnqualifiedGroup { get; set; }

        ///// <summary>
        ///// 不合格代码
        ///// </summary>
        //public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 创建时间数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 处置时间数组 ：时间范围 
        /// </summary>
        public DateTime[]? DisposalTime { get; set; }

        /// <summary>
        /// 不良状态;1、打开 2、关闭
        /// </summary>
        public QualMaterialUnqualifiedStatusEnum? UnqualifiedStatus { get; set; }

        /// <summary>
        /// 处置结果;1、放行 2、退料
        /// </summary>
        public QualMaterialDisposalResultEnum? DisposalResult { get; set; }
    }
}
