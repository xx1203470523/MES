using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（单位维护）   
    /// inte_unit
    /// @author Kaomeng
    /// @date 2023-07-25 08:34:41
    /// </summary>
    public class InteUnitEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 单位名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
