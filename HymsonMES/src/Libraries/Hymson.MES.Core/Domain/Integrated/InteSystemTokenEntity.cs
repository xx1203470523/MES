/*
 *creator: Karl
 *
 *describe: 系统Token    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */
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
        /// 设备名称
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
