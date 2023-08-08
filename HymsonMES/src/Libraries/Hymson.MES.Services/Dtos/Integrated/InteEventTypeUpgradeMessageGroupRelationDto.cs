using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 事件升级消息组关联表新增/更新Dto
    /// </summary>
    public record InteEventTypeUpgradeMessageGroupRelationSaveDto : BaseEntityDto
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
        /// 消息组
        /// </summary>
        public long MessageGroupId { get; set; }

        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public IEnumerable<PushTypeEnum> PushTypes { get; set; }

    }

    /// <summary>
    /// 事件升级消息组关联表Dto
    /// </summary>
    public record InteEventTypeUpgradeMessageGroupRelationDto : BaseEntityDto
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
        /// 消息组
        /// </summary>
        public long MessageGroupId { get; set; }

        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public IEnumerable<PushTypeEnum> PushTypes { get; set; }

    }

    /// <summary>
    /// 事件升级消息组关联表分页Dto
    /// </summary>
    public class InteEventTypeUpgradeMessageGroupRelationPagedQueryDto : PagerInfo { }

}
