using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备基础信息数据实体对象
    /// @author Czhipu
    /// @date 2023-02-08
    /// </summary>
    public class EquEquipmentEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :使用状态 
        /// 空值 : false  
        /// </summary>
        public EquipmentUseStatusEnum UseStatus { get; set; }

        /// <summary>
        /// 描述 :功率 
        /// 空值 : true  
        /// </summary>
        public string Power { get; set; }

        /// <summary>
        /// 描述 :能耗等级 
        /// 空值 : true  
        /// </summary>
        public string EnergyLevel { get; set; }

        /// <summary>
        /// 描述 :ip地址 
        /// 空值 : true  
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 站点ID 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :节拍时间(秒) 
        /// 空值 : true  
        /// </summary>
        public int? TakeTime { get; set; }

        /// <summary>
        /// 描述 :设备编码 
        /// 空值 : false  
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 描述 :设备名称 
        /// 空值 : false  
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 描述 :设备组id 
        /// 空值 : false  
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 描述 :设备描述 
        /// 空值 : true  
        /// </summary>
        public string EquipmentDesc { get; set; }

        /// <summary>
        /// 描述 :工作中心工厂id 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterFactoryId { get; set; }

        /// <summary>
        /// 描述 :工作中心车间id 
        /// 空值 : true  
        /// </summary>
        public long? WorkCenterShopId { get; set; }

        /// <summary>
        /// 描述 :工作中心产线id 
        /// 空值 : true  
        /// </summary>
        public long? WorkCenterLineId { get; set; }

        /// <summary>
        /// 描述 :位置 
        /// 空值 : false  
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 描述 :设备类型 
        /// 空值 : true  
        /// </summary>
        public EquipmentTypeEnum? EquipmentType { get; set; }

        /// <summary>
        /// 描述 :使用部门 
        /// 空值 : true  
        /// </summary>
        public long? UseDepartment { get; set; }

        /// <summary>
        /// 描述 :入场日期 
        /// 空值 : true  
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// 描述 :质保期限（月） 
        /// 空值 : true  
        /// </summary>
        public int QualTime { get; set; }

        /// <summary>
        /// 描述 :过期时间，根据进厂日期+质保日期得出 
        /// 空值 : true  
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 描述 :厂商 
        /// 空值 : true  
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 描述 :供应商 
        /// 空值 : true  
        /// </summary>
        public string Supplier { get; set; }
    }

    /// <summary>
    /// 设备基础信息数据实体对象
    /// @author Czhipu
    /// @date 2023-02-08
    /// </summary>
    public class EquEquipmentView : BaseEntity
    {
        /// <summary>
        /// 描述 :使用状态 
        /// 空值 : false  
        /// </summary>
        public int UseStatus { get; set; }

        /// <summary>
        /// 描述 :设备类型 
        /// 空值 : true  
        /// </summary>
        public int EquipmentType { get; set; }

        /// <summary>
        /// 描述 :使用部门 
        /// 空值 : true  
        /// </summary>
        public int UseDepartment { get; set; }

        /// <summary>
        /// 描述 :功率 
        /// 空值 : true  
        /// </summary>
        public string Power { get; set; } = "";

        /// <summary>
        /// 描述 :能耗等级 
        /// 空值 : true  
        /// </summary>
        public string EnergyLevel { get; set; } = "";

        /// <summary>
        /// 描述 :ip地址 
        /// 空值 : true  
        /// </summary>
        public string Ip { get; set; } = "";

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :节拍时间(秒) 
        /// 空值 : true  
        /// </summary>
        public int? TakeTime { get; set; }

        /// <summary>
        /// 描述 :设备编码 
        /// 空值 : false  
        /// </summary>
        public string EquipmentCode { get; set; } = "";

        /// <summary>
        /// 描述 :设备名称 
        /// 空值 : false  
        /// </summary>
        public string EquipmentName { get; set; } = "";

        /// <summary>
        /// 描述 :设备组id 
        /// 空值 : false  
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 描述 :设备描述 
        /// 空值 : true  
        /// </summary>
        public string EquipmentDesc { get; set; } = "";

        /// <summary>
        /// 描述 :工作中心工厂id 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterFactoryId { get; set; }

        /// <summary>
        /// 描述 :工作中心车间id 
        /// 空值 : true  
        /// </summary>
        public long? WorkCenterShopId { get; set; }

        /// <summary>
        /// 描述 :工作中心产线id 
        /// 空值 : true  
        /// </summary>
        public long? WorkCenterLineId { get; set; }

        /// <summary>
        /// 描述 :位置 
        /// 空值 : false  
        /// </summary>
        public string Location { get; set; } = "";

        /// <summary>
        /// 描述 :入场日期 
        /// 空值 : true  
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// 描述 :质保期限（月） 
        /// 空值 : true  
        /// </summary>
        public int QualTime { get; set; }

        /// <summary>
        /// 描述 :过期时间，根据进厂日期+质保日期得出 
        /// 空值 : true  
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 描述 :厂商 
        /// 空值 : true  
        /// </summary>
        public string Manufacturer { get; set; } = "";

        /// <summary>
        /// 描述 :供应商 
        /// 空值 : true  
        /// </summary>
        public string Supplier { get; set; } = "";


        /// <summary>
        /// 设备组名称
        /// </summary>
        public string EquipmentGroupName { get; set; } = "";
    }
}