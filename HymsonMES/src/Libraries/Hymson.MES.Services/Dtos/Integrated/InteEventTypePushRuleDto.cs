using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 事件类型推送规则新增/更新Dto
    /// </summary>
    public record InteEventTypePushRuleSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 事件类型id
        /// </summary>
        public long? EventTypeId { get; set; }

       /// <summary>
        /// 推送场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public bool PushScene { get; set; }

    }

    /// <summary>
    /// 事件类型推送规则Dto
    /// </summary>
    public record InteEventTypePushRuleDto : BaseEntityDto
    {
       /// <summary>
        /// 推送场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public bool PushScene { get; set; }
       
    }

    /// <summary>
    /// 事件类型推送规则分页Dto
    /// </summary>
    public class InteEventTypePushRulePagedQueryDto : PagerInfo { }

}
