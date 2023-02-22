using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 
    /// </summary>
    public record EquEquipmentUnitCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 单位编码 
        /// </summary>
        public string UnitCode { get; set; } = "";

        /// <summary>
        /// 单位名称 
        /// </summary>
        public string UnitName { get; set; } = "";

        /// <summary>
        /// 单位类型
        /// </summary>
        public int Type { get; set; } = 0;

        /// <summary>
        /// 单位状态 
        /// </summary>
        public int Status { get; set; } = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public record EquEquipmentUnitModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 单位编码 
        /// </summary>
        public string UnitCode { get; set; } = "";

        /// <summary>
        /// 单位名称 
        /// </summary>
        public string UnitName { get; set; } = "";

        /// <summary>
        /// 单位类型
        /// </summary>
        public int Type { get; set; } = 0;

        /// <summary>
        /// 单位状态 
        /// </summary>
        public int Status { get; set; } = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public record EquEquipmentUnitDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 单位编码 
        /// </summary>
        public string UnitCode { get; set; } = "";

        /// <summary>
        /// 单位名称 
        /// </summary>
        public string UnitName { get; set; } = "";

        /// <summary>
        /// 单位类型
        /// </summary>
        public int Type { get; set; } = 0;

        /// <summary>
        /// 单位状态 
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; } = "";

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentUnitPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public string? SiteCode { get; set; }

        /// <summary>
        /// 单位编码 
        /// </summary>
        public string? UnitCode { get; set; }

        /// <summary>
        /// 单位名称 
        /// </summary>
        public string? UnitName { get; set; }

        /// <summary>
        /// 单位类型
        /// </summary>
        public int Type { get; set; } = 0;

        /// <summary>
        /// 单位状态 
        /// </summary>
        public int Status { get; set; } = 0;
    }
}
