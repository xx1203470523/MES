using Hymson.Infrastructure;

namespace  Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体对象（异常消息组）
    /// @author Czhipu
    /// @date 2022-11-17
    /// </summary>
    public class InteAbnormalMessageGroup : BaseEntity
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 异常消息组编码 
        /// </summary>
        public string AbnormalMessageGroupCode { get; set; }

        /// <summary>
        /// 异常消息组名 
        /// </summary>
        public string AbnormalMessageGroupName { get; set; }

        /// <summary>
        /// 工作中心车间id
        /// </summary>
        public long WorkCenterShopId { get; set; }

        /// <summary>
        /// 已经存在消息类型（一个异常消息组只能有一种异常消息类型，用于校验消息类型是否存在）
        /// </summary>
        public string ExistMessageType { get; set; }
    }
}
