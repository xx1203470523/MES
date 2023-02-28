using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// Dto（设备故障现象）
    /// </summary>
    public record EquFaultPhenomenonDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string FaultPhenomenonCode { get; set; } = "";

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string FaultPhenomenonName { get; set; } = "";

        /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 使用状态 0-禁用 1-启用
        /// </summary>
        public int UseStatus { get; set; }

        /// <summary>
        /// 设备故障先
        /// </summary>
        public string Remark { get; set; } = "";


        /// <summary>
        /// 设备组名称
        /// </summary>
        public string EquipmentGroupName { get; set; } = "";
    }


    /// <summary>
    /// 新增Dto（设备故障现象）
    /// </summary>
    public record EquFaultPhenomenonCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string FaultPhenomenonCode { get; set; } = "";

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string FaultPhenomenonName { get; set; } = "";

        /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 使用状态 0-禁用 1-启用
        /// </summary>
        public int UseStatus { get; set; }

        /// <summary>
        /// 设备故障先
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 更新Dto（设备故障现象）
    /// </summary>
    public record EquFaultPhenomenonModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string FaultPhenomenonCode { get; set; } = "";

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string FaultPhenomenonName { get; set; } = "";

        /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 使用状态 0-禁用 1-启用
        /// </summary>
        public int UseStatus { get; set; }

        /// <summary>
        /// 设备故障先
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 删除Dto（设备故障现象）
    /// </summary>
    public record EquFaultPhenomenonDeleteDto
    {
        /// <summary>
        /// 集合（主键）
        /// </summary>
        public long[] Ids { get; set; }
    }


    /// <summary>
    /// 分页Dto（设备故障现象）
    /// </summary>
    public class EquFaultPhenomenonPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string? FaultPhenomenonCode { get; set; }

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string? FaultPhenomenonName { get; set; }

        /// <summary>
        /// 使用状态 0-禁用 1-启用
        /// </summary>
        public int? UseStatus { get; set; }

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string? EquipmentGroupName { get; set; }
    }
}
