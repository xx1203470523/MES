using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 备件注册表 分页参数
    /// </summary>
    public class EquSparePartsPagedQuery : PagerInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string? SparePartsGroup { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get;set; }

    }
}
