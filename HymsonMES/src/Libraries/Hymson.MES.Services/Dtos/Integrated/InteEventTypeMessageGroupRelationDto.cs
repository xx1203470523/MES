using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 事件类型关联群组新增/更新Dto
    /// </summary>
    public record InteEventTypeMessageGroupRelationSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 消息组id
        /// </summary>
        public long MessageGroupId { get; set; }

        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public IEnumerable<PushTypeEnum> PushTypes { get; set; }

    }

    /// <summary>
    /// 事件类型关联群组分页Dto
    /// </summary>
    public class InteEventTypeMessageGroupRelationPagedQueryDto : PagerInfo { }

}
