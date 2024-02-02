using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 时间通配（转换），数据实体对象   
    /// inte_time_wildcard
    /// @author Karl
    /// @date 2023-10-13 06:33:21
    /// </summary>
    public class InteTimeWildcardEntity : BaseEntity
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 类型： 年  月 日
        /// </summary>
        public TimeWildcardTypeEnum Type { get; set; }

       /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
