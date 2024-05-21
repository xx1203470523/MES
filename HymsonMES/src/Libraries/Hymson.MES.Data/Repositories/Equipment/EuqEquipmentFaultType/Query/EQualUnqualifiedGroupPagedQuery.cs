using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>   
    /// 不合格代码组分页参数
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class EQualUnqualifiedGroupPagedQuery : PagerInfo
    {
        // <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 故障类型编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 故障类型名称
        /// </summary>
        public string? Name { get; set; }


        /// <summary>
        /// 装态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }
}
