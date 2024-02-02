using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 系统Token，数据实体对象   
    /// inte_system_token
    /// @author zhaoqing
    /// @date 2023-06-15 02:09:57
    /// </summary>
    public class InteSystemTokenEntity : BaseEntity
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

       /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

       /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }       
    }
}
