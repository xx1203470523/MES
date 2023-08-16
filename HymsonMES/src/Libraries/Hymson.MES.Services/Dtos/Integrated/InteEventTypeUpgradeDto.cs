using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 事件升级新增/更新Dto
    /// </summary>
    public record InteEventTypeUpgradeSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 升级场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public PushSceneEnum PushScene { get; set; }

        /// <summary>
        /// 级别;1、第一等级2、第二等级3、第三等级
        /// </summary>
        public UpgradeLevelEnum Level { get; set; }

        /// <summary>
        /// 升级时长
        /// </summary>
        public int Duration { get; set; }

    }

    /// <summary>
    /// 事件升级Dto
    /// </summary>
    public record InteEventTypeUpgradeDto : BaseEntityDto
    {
        /// <summary>
        /// 级别;1、第一等级2、第二等级3、第三等级
        /// </summary>
        public UpgradeLevelEnum Level { get; set; }

        /// <summary>
        /// 升级时长
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 关联消息组
        /// </summary>
        public IEnumerable<InteEventTypeMessageGroupRelationDto>? MessageGroups { get; set; }
    }

    /// <summary>
    /// 事件升级分页Dto
    /// </summary>
    public class InteEventTypeUpgradePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 时间类型ID
        /// </summary>
        public long EventTypeId { get; set; }

        /// <summary>
        /// 推送场景
        /// </summary>
        public PushSceneEnum PushScene { get; set; }
    }

}
