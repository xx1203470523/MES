using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.Core.NIO
{
    /// <summary>
    /// 数据实体（蔚来推送开关）   
    /// nio_push_switch
    /// @author Czhipu
    /// @date 2024-07-10 08:37:00
    /// </summary>
    public class NioPushSwitchEntity : BaseEntity
    {
        /// <summary>
        /// 场景代码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 请求路径（0:Get;1:Post;2:Put;3:Delete;）
        /// </summary>
        public int Method { get; set; }

        /// <summary>
        /// 业务场景;0：表示总开关；
        /// </summary>
        public BuzSceneEnum BuzScene { get; set; }

        /// <summary>
        /// 是否启用推送;0：不推送；1：推送；
        /// </summary>
        public TrueOrFalseEnum IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
