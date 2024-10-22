namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 消息组推送方式 查询参数
    /// </summary>
    public class InteMessageGroupPushMethodQuery
    {
        /// <summary>
        /// 集合（消息组Id）
        /// </summary>
        public long[] MessageGroupIds { get; set; }
    }
}
