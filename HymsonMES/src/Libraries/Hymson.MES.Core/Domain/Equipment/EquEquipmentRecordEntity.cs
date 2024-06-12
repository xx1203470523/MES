/*
 *creator: Karl
 *
 *describe: 设备台账信息    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-06-12 10:53:50
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquEquipmentRecord
{
    /// <summary>
    /// 设备台账信息，数据实体对象   
    /// equ_equipment_record
    /// @author pengxin
    /// @date 2024-06-12 10:53:50
    /// </summary>
    public class EquEquipmentRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备 idequ_equipment的id
        /// </summary>
        public string EquipmentId { get; set; }

       /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

       /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

       /// <summary>
        /// 设备组id
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// 设备描述
        /// </summary>
        public string EquipmentDesc { get; set; }

       /// <summary>
        /// 工作中心工厂id  (废弃)
        /// </summary>
        public long WorkCenterFactoryId { get; set; }

       /// <summary>
        /// 工作中心车间id
        /// </summary>
        public long? WorkCenterShopId { get; set; }

       /// <summary>
        /// 工作中心产线id
        /// </summary>
        public long? WorkCenterLineId { get; set; }

       /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// 设备类型
        /// </summary>
        public bool? EquipmentType { get; set; }

       /// <summary>
        /// 使用部门
        /// </summary>
        public long? UseDepartment { get; set; }

       /// <summary>
        /// 使用状态
        /// </summary>
        public bool UseStatus { get; set; }

       /// <summary>
        /// 入场日期
        /// </summary>
        public DateTime? EntryDate { get; set; }

       /// <summary>
        /// 质保期限（月）
        /// </summary>
        public int? QualTime { get; set; }

       /// <summary>
        /// 过期时间，根据进厂日期+质保日期得出
        /// </summary>
        public DateTime? ExpireDate { get; set; }

       /// <summary>
        /// 厂商
        /// </summary>
        public string Manufacturer { get; set; }

       /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

       /// <summary>
        /// 功率
        /// </summary>
        public string Power { get; set; }

       /// <summary>
        /// 能耗等级
        /// </summary>
        public string EnergyLevel { get; set; }

       /// <summary>
        /// ip地址
        /// </summary>
        public string Ip { get; set; }

       /// <summary>
        /// 节拍时间(秒)
        /// </summary>
        public int? TakeTime { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 操作类型;1、设备注册2、设备点检3，设备保养4，设备维修，5、备件绑定6、备件解绑
        /// </summary>
        public string OperationType { get; set; }

       
    }
}
