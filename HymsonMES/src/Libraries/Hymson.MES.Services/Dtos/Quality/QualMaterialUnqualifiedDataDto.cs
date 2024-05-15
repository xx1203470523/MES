using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 车间物料不良记录新增/更新Dto
    /// </summary>
    public record QualMaterialUnqualifiedDataSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 物料库存Id;wh_material_inventory的Id
        /// </summary>
        public long MaterialInventoryId { get; set; }

       /// <summary>
        /// 不良状态;1、打开 2、关闭
        /// </summary>
        public bool UnqualifiedStatus { get; set; }

       /// <summary>
        /// 不良备注
        /// </summary>
        public string UnqualifiedRemark { get; set; }

       /// <summary>
        /// 处置结果;1、放行 2、退料
        /// </summary>
        public bool? DisposalResult { get; set; }

       /// <summary>
        /// 处置时间
        /// </summary>
        public DateTime? DisposalTime { get; set; }

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
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 车间物料不良记录Dto
    /// </summary>
    public record QualMaterialUnqualifiedDataDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 物料库存Id;wh_material_inventory的Id
        /// </summary>
        public long MaterialInventoryId { get; set; }

       /// <summary>
        /// 不良状态;1、打开 2、关闭
        /// </summary>
        public bool UnqualifiedStatus { get; set; }

       /// <summary>
        /// 不良备注
        /// </summary>
        public string UnqualifiedRemark { get; set; }

       /// <summary>
        /// 处置结果;1、放行 2、退料
        /// </summary>
        public bool? DisposalResult { get; set; }

       /// <summary>
        /// 处置时间
        /// </summary>
        public DateTime? DisposalTime { get; set; }

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
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 车间物料不良记录分页Dto
    /// </summary>
    public class QualMaterialUnqualifiedDataPagedQueryDto : PagerInfo { }

}
