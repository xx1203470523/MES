using Hymson.Infrastructure;

namespace Hymson.MES.CoreServices.Bos.Integrated
{
    /// <summary>
    /// 消息组关联Bo
    /// </summary>
    public record MessageGroupBo : BaseEntityDto
    {
        // TODO 应该新建一个实体转BO对象的基类 BaseEntityBo

        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 消息组id
        /// </summary>
        public long MessageGroupId { get; set; }

        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public string PushTypes { get; set; } = "";
    }
}
