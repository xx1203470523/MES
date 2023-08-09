using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 事件类型维护新增/更新Dto
    /// </summary>
    public record InteEventTypeSaveDto : BaseEntityDto
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
        /// 车间id
        /// </summary>
        public long WorkShopId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 关联消息组
        /// </summary>
        public IEnumerable<InteEventTypeMessageGroupRelationDto>? MessageGroups { get; set; }

        /// <summary>
        /// 接收升级
        /// </summary>
        public IEnumerable<InteEventTypeUpgradeDto> ReceiveUpgrades { get; set; }

        /// <summary>
        /// 处理升级
        /// </summary>
        public IEnumerable<InteEventTypeUpgradeDto> HandleUpgrades { get; set; }

        /// <summary>
        /// 推送规则
        /// </summary>
        public IEnumerable<InteEventTypePushRuleDto> Rules { get; set; }

    }

    /// <summary>
    /// 事件类型维护Dto
    /// </summary>
    public record InteEventTypeDto : BaseEntityDto
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
        /// 车间id
        /// </summary>
        public long WorkShopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
    /// 事件类型维护分页Dto
    /// </summary>
    public class InteEventTypePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string? WorkShopName { get; set; }
    }

    /// <summary>
    /// 事件类型关联群组Dto
    /// </summary>
    public record InteEventTypeMessageGroupRelationDto : BaseEntityDto
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
        /// 消息组id
        /// </summary>
        public long MessageGroupId { get; set; }

        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public IEnumerable<PushTypeEnum> PushTypeArray { get; set; }

    }

}
