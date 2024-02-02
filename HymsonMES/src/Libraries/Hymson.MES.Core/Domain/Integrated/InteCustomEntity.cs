using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 客户维护，数据实体对象   
    /// inte_custom
    /// @author Karl
    /// @date 2023-07-11 09:33:26
    /// </summary>
    public class InteCustomEntity : BaseEntity
    {
        /// <summary>
        /// 客户编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

       /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

       /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
