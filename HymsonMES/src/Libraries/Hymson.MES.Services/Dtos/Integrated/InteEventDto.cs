using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 事件维护新增/更新Dto
    /// </summary>
    public record InteEventSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 事件类型id
        /// </summary>
        public long? EventTypeId { get; set; }
        public string? EventTypeCode { get; set; }

        /// <summary>
        /// 状态;0、禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 是否自动关闭(0、否 1、是)
        /// </summary>
        public DisableOrEnableEnum IsAutoClose { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";

    }

    /// <summary>
    /// 事件维护Dto
    /// </summary>
    public record InteEventInfoDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 事件类型id
        /// </summary>
        public long EventTypeId { get; set; }

        /// <summary>
        /// 事件类型编码
        /// </summary>
        public string EventTypeCode { get; set; }

        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string EventTypeName { get; set; } = "";

        /// <summary>
        /// 状态;0、禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 是否自动关闭(0、否 1、是)
        /// </summary>
        public DisableOrEnableEnum IsAutoClose { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 事件维护Dto
    /// </summary>
    public record InteEventDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 事件类型id
        /// </summary>
        public long EventTypeId { get; set; }

        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string EventTypeName { get; set; }

        /// <summary>
        /// 状态;0、禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 是否自动关闭(0、否 1、是)
        /// </summary>
        public DisableOrEnableEnum IsAutoClose { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 自定义实体列表（事件）
    /// </summary>
    public record InteEventBaseDto : BaseEntityDto
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
        /// 事件类型ID
        /// </summary>
        public long? EventTypeId { get; set; }
    }

    /// <summary>
    /// 事件维护分页Dto
    /// </summary>
    public class InteEventPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 事件类型Id
        /// </summary>
        public long? EventTypeId { get; set; }

        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string? EventTypeName { get; set; }

    }

}
