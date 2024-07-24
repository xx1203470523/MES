using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Core.Enums.Plan;

namespace Hymson.MES.Core.NIO
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
        public BuzSceneEnum BuzScene { get; set; }

        /// <summary>
        /// 推送状态;1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum Status { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public string? Result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }
}
