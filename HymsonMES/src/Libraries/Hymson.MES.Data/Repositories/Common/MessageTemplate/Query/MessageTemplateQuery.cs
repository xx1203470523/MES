
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 消息模板 查询参数
    /// </summary>
    public class MessageTemplateQuery : EntityBySiteIdQuery
    {
        /// <summary>
        /// 业务类型;1、异常消息2、待定
        /// </summary>
        public BusinessTypeEnum BusinessType { get; set; }
    }
}
