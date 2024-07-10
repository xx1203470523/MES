using Hymson.Infrastructure;

namespace Hymson.MES.BackgroundServices.NIO
{
    /// <summary>
    /// 数据实体（蔚来推送表）   
    /// nio_push
    /// @author Czhipu
    /// @date 2024-07-10 08:37:18
    /// </summary>
    public class NioPushEntity : BaseEntity
    {
        /// <summary>
        /// 场景代码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 业务场景;这里不允许0的数据存在；
        /// </summary>
        public int BuzScene { get; set; }

        /// <summary>
        /// 推送状态;1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public bool Status { get; set; }

    }
}
