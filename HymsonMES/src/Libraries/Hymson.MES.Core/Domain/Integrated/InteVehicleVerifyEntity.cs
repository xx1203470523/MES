/*
 *creator: Karl
 *
 *describe: 载具校验    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-07-17 09:34:37
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 载具校验，数据实体对象   
    /// inte_vehicle_verify
    /// @author Karl
    /// @date 2023-07-17 09:34:37
    /// </summary>
    public class InteVehicleVerifyEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 载具id
        /// </summary>
        public long VehicleId { get; set; }

       /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

       
    }
}
