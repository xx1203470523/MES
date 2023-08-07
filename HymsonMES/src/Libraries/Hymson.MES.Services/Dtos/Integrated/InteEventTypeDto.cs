using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 事件维护新增/更新Dto
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
        public IEnumerable<InteEventTypeMessageGroupRelationDto> MessageGroups { get; set; }

        /// <summary>
        /// 关联消息组
        /// </summary>
        public IEnumerable<InteEventTypeUpgradeDto> Receives { get; set; }

        /// <summary>
        /// 关联消息组
        /// </summary>
        public IEnumerable<InteEventTypeUpgradeDto> Handles { get; set; }

        /// <summary>
        /// 关联消息组
        /// </summary>
        public IEnumerable<InteEventTypePushRuleDto> Rules { get; set; }

    }

    /// <summary>
    /// 事件维护Dto
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
    /// 事件维护分页Dto
    /// </summary>
    public class InteEventTypePagedQueryDto : PagerInfo { }

}
