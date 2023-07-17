/*
 *creator: Karl
 *
 *describe: 载具类型验证    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-07-13 03:15:22
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 载具类型验证，数据实体对象   
    /// inte_vehicle_type_verify
    /// @author Karl
    /// @date 2023-07-13 03:15:22
    /// </summary>
    public class InteVehicleTypeVerifyEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 载具类型id
        /// </summary>
        public long VehicleTypeId { get; set; }

       /// <summary>
        /// 验证类型;1-物料  2-物料组
        /// </summary>
        public VehicleTypeVerifyTypeEnum Type { get; set; }

       /// <summary>
        /// 物料或者物料组id
        /// </summary>
        public long VerifyId { get; set; }
    }
}
