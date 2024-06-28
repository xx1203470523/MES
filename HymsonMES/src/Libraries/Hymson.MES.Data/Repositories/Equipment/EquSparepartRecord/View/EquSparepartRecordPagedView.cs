/*
 *creator: Karl
 *
 *describe: 设备维修记录 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.EquSparepartRecord
{
    /// <summary>
    /// 备件记录
    /// </summary>
    public class EquSparepartRecordPagedView : BaseEntity
    {
        /// <summary>
        /// 备件编号 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparePartTypeCode { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

        /// <summary>
        /// 操作数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 操作数量
        /// </summary>
        public int OperationQty { get; set; } 

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string RecordWorkCenterCode { get; set; } 

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
        /// 位置
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 操作类型 1、注册 2、入库 3、出库4、绑定5、解绑
        /// </summary> 
        public EquOperationTypeEnum? OperationType { get; set; }

        /// <summary>
        /// 领用人
        /// </summary>
        public string Recipients { get; set; } 

    }
}
