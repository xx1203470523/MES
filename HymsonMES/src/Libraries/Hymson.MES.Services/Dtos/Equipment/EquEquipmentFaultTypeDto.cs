using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 不合格代码组Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record EquipmentFaultTypeDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备故障编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备故障名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 转台
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }
  
    }

    /// <summary>
    /// 设备故障关联现象
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record EquipmentFaultTypePhenomenonRelationDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格代码组id
        /// </summary>
        public long UnqualifiedGroupId { get; set; }


        /// <summary>
        /// 不合格代码
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 故障类型ID;equ_fault_type的Id
        /// </summary>
        public long FaultTypeId { get; set; }


        /// <summary>
        /// 故障现象ID;equ_fault_phenomenon的Id
        /// </summary>
        public long FaultPhenomenonId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string Name { get; set; }

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
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 设备故障关联设备组
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record EquipmentFaultTypeEquipmentGroupRelationDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备故障编码
        /// </summary>
        public long FaultTypeId { get; set; }

        /// <summary>
        /// 设备故障名称
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 不合格代码组id
        /// </summary>
        public long UnqualifiedGroupId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureCode { get; set; }

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
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 不合格代码组新增Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record EQualUnqualifiedGroupCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 设备故障编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备故障名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 设备故障类型状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 不合格组
        /// </summary>
        public string? UnqualifiedGroup { get; set; }

        /// <summary>
        /// 不合格组名称
        /// </summary>
        public string? UnqualifiedGroupName { get; set; }
        

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 不合格代码id
        /// </summary>
        public List<long>? UnqualifiedCodeIds { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public List<long>? ProcedureIds { get; set; }
    }

    /// <summary>
    /// 不合格代码组更新Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public record EQualUnqualifiedGroupModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 不合格组名称
        /// </summary>
        public string? UnqualifiedGroupName { get; set; }

        /// <summary>
        /// 设备故障名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 设备故障类型状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 不和代码id
        /// </summary>
        public List<long> UnqualifiedCodeIds { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public List<long> ProcedureIds { get; set; }
    }

    /// <summary>
    /// 不合格代码组查询Dto
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class EquipmentFaultTypePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 不合格组
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 不合格组名称
        /// </summary>
        public string? Name { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }

    /// <summary>
    /// 不合格组查询对象
    /// </summary>
    public class EQualUnqualifiedGroupQueryDto
    {
        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }
    }
}
