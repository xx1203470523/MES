using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.BackgroundServices.NIO
{
    /// <summary>
    /// 数据实体（蔚来推送项目表）   
    /// nio_push_item
    /// @author Czhipu
    /// @date 2024-07-10 08:37:27
    /// </summary>
    public class NioPushItemEntity : BaseEntity
    {
        /// <summary>
        /// 业务场景;这里不允许0的数据存在；
        /// </summary>
        public BuzSceneEnum BuzScene { get; set; }

        /// <summary>
        /// 业务类型;1：转子线；2：定子线；
        /// </summary>
        public BuzTypeEnum BuzType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否组装;0：待组装；1：已组装；
        /// </summary>
        public TrueOrFalseEnum IsAssembled { get; set; }

    }
}
