/*
 *creator: Karl
 *
 *describe: 设备维修记录 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Data.Repositories.EquSparepartRecord
{
    /// <summary>
    /// 设备台账
    /// </summary>
    public class EquEquipmentRecordPagedView : BaseEntity
    {


        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 资源名称 
        /// </summary>
        public string ResName { get; set; }


        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 操作类型;1、设备注册2、设备点检3，设备保养4，设备维修，5、备件绑定6、备件解绑
        /// </summary>
        public EquEquipmentRecordOperationTypeEnum OperationType { get; set; }


    }
}
