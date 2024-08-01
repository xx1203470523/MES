using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 资源设备绑定表Dto
    /// </summary>
    public record ProcResourceEquipmentBindDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 是否主设备
        /// </summary>
        public TrueOrFalseEnum? IsMain { get; set; }

        /// <summary>
        /// 备注
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

    public record ProcResourceEquipmentBindViewDto : ProcResourceEquipmentBindDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备描述
        /// </summary>
        public string EquipmentDesc { get; set; }
    }

    /// <summary>
    /// 资源设备绑定表新增Dto
    /// </summary>
    public record ProcResourceEquipmentBindCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 是否主设备
        /// </summary>
        public TrueOrFalseEnum IsMain { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }
    }

    /// <summary>
    /// 资源设备绑定表更新Dto
    /// </summary>
    public record ProcResourceEquipmentBindModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 是否主设备
        /// </summary>
        public TrueOrFalseEnum IsMain { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

    }

    /// <summary>
    /// 资源设备绑定表分页Dto
    /// </summary>
    public class ProcResourceEquipmentBindPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }
}
