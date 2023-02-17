using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体对象（工装类型）
    /// equ_consumable_type
    /// @author 陈志谱
    /// @date 2023-02-17
    /// </summary>
    public class EquConsumableTypeEntity : BaseEntity, ISite
    {
        /// <summary>
        /// 工装类型编码
        /// </summary>
        public string ConsumableTypeCode { get; set; } = "";

        /// <summary>
        /// 工装类型名称
        /// </summary>
        public string ConsumableTypeName { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 站点Id
        /// </summary>
        long ISite.SiteId { get; }
    }
}
