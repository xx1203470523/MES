using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 保存Dto（单位维护）
    /// </summary>
    public record EquEquipmentUnitSaveDto : BaseEntityDto
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
        public int Type { get; set; } = DbDefaultValueConstant.IntDefaultValue;

        /// <summary>
        /// 单位状态 
        /// </summary>
        public int Status { get; set; } = DbDefaultValueConstant.IntDefaultValue;

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";
    }

    /// <summary>
    ///  Dto（单位维护）
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
        public int Type { get; set; } = DbDefaultValueConstant.IntDefaultValue;

        /// <summary>
        /// 单位状态 
        /// </summary>
        public int Status { get; set; } = DbDefaultValueConstant.IntDefaultValue;

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
    /// 分页Dto（单位维护）
    /// </summary>
    public class EquEquipmentUnitPagedQueryDto : PagerInfo
    {
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
        public int? Type { get; set; }

        /// <summary>
        /// 单位状态 
        /// </summary>
        public int? Status { get; set; }
    }
}
