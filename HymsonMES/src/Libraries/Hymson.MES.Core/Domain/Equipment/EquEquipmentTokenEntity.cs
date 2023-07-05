/*
 *creator: Karl
 *
 *describe: 设备Token    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-06-07 02:17:26
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备Token，数据实体对象   
    /// equ_equipment_token
    /// @author pengxin
    /// @date 2023-06-07 02:17:26
    /// </summary>
    public class EquEquipmentTokenEntity : BaseEntity
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

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