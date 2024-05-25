/*
 *creator: Karl
 *
 *describe: 设备验证    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-28 09:02:39
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备验证Dto
    /// </summary>
    public record EquEquipmentVerifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public EquVerifyAccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       
    }


    /// <summary>
    /// 设备验证新增Dto
    /// </summary>
    public record EquEquipmentVerifyCreateDto : BaseEntityDto
    {
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

    /// <summary>
    /// 设备验证分页Dto
    /// </summary>
    public class EquEquipmentVerifyPagedQueryDto : PagerInfo
    {
    }
}
