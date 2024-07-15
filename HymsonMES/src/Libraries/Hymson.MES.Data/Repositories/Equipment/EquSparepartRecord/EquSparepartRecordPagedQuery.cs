/*
 *creator: Karl
 *
 *describe: 设备备件记录表 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:29:55
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.EquSparepartRecord
{
    /// <summary>
    /// 设备备件记录表 分页参数
    /// </summary>
    public class EquSparepartRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// SiteId
        /// </summary>
        public long? SiteId { get; set; }
        /// <summary>
        /// 备件编号 
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string? SparePartTypeCode { get; set; }

        /// <summary>
        /// 操作类型 1、注册 2、入库 3、出库4、绑定5、解绑
        /// </summary> 
        public EquOperationTypeEnum? OperationType { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }
    }
}
