using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 查询实体
    /// </summary>
    public class EntityByStatusQuery : EntityBySiteIdQuery
    {
        /// <summary>
        /// 状态枚举
        /// </summary>
        public IEnumerable<SysDataStatusEnum> StatusEnums { get; set; } = new List<SysDataStatusEnum> { SysDataStatusEnum.Enable, SysDataStatusEnum.Retain };

    }
}
