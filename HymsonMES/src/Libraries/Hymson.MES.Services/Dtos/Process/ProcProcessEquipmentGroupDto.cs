using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 工艺工艺设备组新增/更新Dto
    /// </summary>
    public record ProcProcessEquipmentGroupSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 编码（工艺设备组）
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称（工艺设备组）
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// ID集合（设备注册）
        /// </summary>
        public IEnumerable<string>? EquipmentIDs { get; set; }
    }

    /// <summary>
    /// 工艺设备组Dto
    /// </summary>
    public record ProcProcessEquipmentGroupListDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码（工艺设备组）
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称（工艺设备组）
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 备注
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
    /// 工艺设备组分页Dto
    /// </summary>
    public class ProcProcessEquipmentGroupPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码（工艺设备组）
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称（工艺设备组）
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// 自定义实体对象（工艺设备组）
    /// </summary>
    public class ProcProcessEquipmentGroupDto
    {
        /// <summary>
        /// 信息（工艺设备组）
        /// </summary>
        public ProcProcessEquipmentGroupListDto? Info { get; set; }

        /// <summary>
        /// 集合（设备）
        /// </summary>
        public IEnumerable<ProcProcessEquipmentBaseDto> Equipments { get; set; }

    }

    /// <summary>
    /// 查询对象（工艺设备组）
    /// </summary>
    public record ProcProcessEquipmentGroupQueryDto : BaseEntityDto
    {
        /// <summary>
        /// 操作类型 1:add；2:edit；3:view；
        /// </summary>
        public OperateTypeEnum OperateType { get; set; } = OperateTypeEnum.Add;

        /// <summary>
        /// 工艺设备组Id
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// 自定义实体列表（设备注册）
    /// </summary>
    public record ProcProcessEquipmentBaseDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码（设备注册）
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称（设备注册）
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 工艺设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }
    }

}