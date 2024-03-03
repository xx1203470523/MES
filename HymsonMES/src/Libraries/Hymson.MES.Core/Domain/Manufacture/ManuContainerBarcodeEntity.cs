/*
 *creator: Karl
 *
 *describe: 容器条码表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using System.Reflection.Metadata.Ecma335;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 容器条码表，数据实体对象   
    /// manu_container_barcode
    /// @author wxk
    /// @date 2023-04-12 02:29:23
    /// </summary>
    public class ManuContainerBarcodeEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 容器规格id
        /// </summary>
        public long ContainerId { get; set; }
        /// <summary>
        /// 包装等级
        /// </summary>
        public int PackLevel { get; set; }

        /// <summary>
        /// 描述 :物料编码 
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }
        /// <summary>
        /// 生产工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 状态;1：打开 2：关闭
        /// </summary>
        public ManuContainerBarcodeStatusEnum Status { get; set; } = ManuContainerBarcodeStatusEnum.Open;

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; } = 0;
    }
}
