/*
 *creator: Karl
 *
 *describe: 设备参数组    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-08-02 01:48:35
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 设备参数组，数据实体对象   
    /// proc_equipment_group_param
    /// @author Karl
    /// @date 2023-08-02 01:48:35
    /// </summary>
    public class ProcEquipmentGroupParamEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 配编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 参数组类型;1、开机参数 2、设备过程参数
        /// </summary>
        public bool Type { get; set; }

       /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 工序Id
        /// </summary>
        public long Procedure { get; set; }

       /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 是否使用0 未使用，1使用
        /// </summary>
        public bool? IsUsed { get; set; }

       
    }
}
