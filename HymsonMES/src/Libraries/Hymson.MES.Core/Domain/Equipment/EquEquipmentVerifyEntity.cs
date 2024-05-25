using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备验证，数据实体对象   
    /// equ_equipment_verify
    /// @author Karl
    /// @date 2023-07-28 09:02:39
    /// </summary>
    public class EquEquipmentVerifyEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public EquVerifyAccountTypeEnum AccountType { get; set; }
    }
}
