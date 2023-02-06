using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.OnStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.OnStock
{
    public class WhStockChangeRecordEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :站点编码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 描述 :变更类型(0-库存异动 1-储位转换 2-形态转换) 
        /// 空值 : false  
        /// </summary>
        public ChangeTypeEnum ChangeType { get; set; }

        /// <summary>
        /// 描述 :来源单号 
        /// 空值 : false  
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 描述 :来源项次 
        /// 空值 : false  
        /// </summary>
        public string SourceItem { get; set; }

        /// <summary>
        /// 描述 :来源单据类型(0-收货 1-上架 2-调拨) 
        /// 空值 : false  
        /// </summary>
        public string SourceBillType { get; set; }

        /// <summary>
        /// 描述 :标签ID 
        /// 空值 : false  
        /// </summary>
        public string LabelId { get; set; }

        /// <summary>
        /// 描述 :物料Id 
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 描述 :料号 
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述 :项目号 
        /// 空值 : false  
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 描述 :库存管理特征 
        /// 空值 : false  
        /// </summary>
        public string StockManageFeature { get; set; }

        /// <summary>
        /// 变更前库存管理特征
        /// </summary>
        public string BeforeStockManageFeature { get; set; }

        /// <summary>
        /// 描述 :旧料号 
        /// 空值 : false  
        /// </summary>
        public string OldMaterialCode { get; set; }

        /// <summary>
        /// 描述 :批次 
        /// 空值 : false  
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 描述 :制造序号SN 
        /// 空值 : false  
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 描述 :单位 
        /// 空值 : false  
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 描述 :变更数量(负数代表库存减少) 
        /// 空值 : false  
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 描述 :仓库 
        /// 空值 : false  
        /// </summary>
        public long WarehouseId { get; set; }

        /// <summary>
        /// 描述 :区域 
        /// 空值 : false  
        /// </summary>
        public long WarehouseAreaId { get; set; }

        /// <summary>
        /// 描述 :货架 
        /// 空值 : false  
        /// </summary>
        public long WarehouseRackId { get; set; }

        /// <summary>
        /// 描述 :储位 
        /// 空值 : false  
        /// </summary>
        public long WarehouseBinId { get; set; }

        /// <summary>
        /// 描述 :容器 
        /// 空值 : false  
        /// </summary>
        public long ContainerId { get; set; }
    }
}
