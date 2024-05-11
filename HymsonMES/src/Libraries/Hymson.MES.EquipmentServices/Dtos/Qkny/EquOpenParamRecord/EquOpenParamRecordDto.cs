using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.EquOpenParamRecord
{
    /// <summary>
    /// 开机参数记录表新增/更新Dto
    /// </summary>
    public record EquOpenParamRecordSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 参数Id
        /// </summary>
        public long? ParamId { get; set; }

       /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; } = string.Empty;

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; } = string.Empty;

       /// <summary>
        /// 批次Id
        /// </summary>
        public long? BatchId { get; set; }

       /// <summary>
        /// 配方Id
        /// </summary>
        public long RecipeId { get; set; }

       /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; } = string.Empty;

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 开机参数记录表Dto
    /// </summary>
    public record EquOpenParamRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = string.Empty;

       /// <summary>
        /// 参数Id
        /// </summary>
        public long? ParamId { get; set; }

       /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; } = string.Empty;

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; } = string.Empty;

       /// <summary>
        /// 批次Id
        /// </summary>
        public long? BatchId { get; set; }

       /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 配方Id
        /// </summary>
        public long RecipeId { get; set; }

       /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; } = string.Empty;

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 开机参数记录表分页Dto
    /// </summary>
    public class EquOpenParamRecordPagedQueryDto : PagerInfo { }

}
