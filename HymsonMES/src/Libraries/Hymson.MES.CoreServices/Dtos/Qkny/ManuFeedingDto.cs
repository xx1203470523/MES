using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.CoreServices.Dtos.Common;

namespace Hymson.MES.CoreServices.Dtos.Qkny
{
    internal class ManuFeedingDto
    {
    }

    /// <summary>
    /// 资源查询对象
    /// </summary>
    public class ManuFeedingResourceQueryDto
    {
        /// <summary>
        /// 查询来源
        /// </summary>
        public FeedingSourceEnum Source { get; set; } = FeedingSourceEnum.Resource;

        /// <summary>
        /// 资源编码/物料编码
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// 物料资源
    /// </summary>
    public class ManuFeedingResourceDto
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
    }

    /// <summary>
    /// 查询对象（上料点）
    /// </summary>
    public class ManuFeedingLoadPointQueryDto
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
    }

    /// <summary>
    /// 查询对象（工单）
    /// </summary>
    public class ManuFeedingWorkOrderQueryDto
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
    }

    /// <summary>
    /// 查询对象（物料）
    /// </summary>
    public class ManuFeedingMaterialQueryDto : CoreBaseDto
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 物料加载来源
        /// </summary>
        public ManuSFCFeedingSourceEnum? Source { get; set; }

        /// <summary>
        /// 上料点ID
        /// </summary>
        public long? FeedingPointId { get; set; }
    }

    /// <summary>
    /// 物料对象
    /// </summary>
    public class ManuFeedingMaterialDto
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 是否历史清单
        /// </summary>
        public bool IsHistory { get; set; } = false;

        /// <summary>
        /// 物料库存集合
        /// </summary>
        public List<ManuFeedingMaterialItemDto> Children { get; set; }
    }

    /// <summary>
    /// 物料库存对象
    /// </summary>
    public class ManuFeedingMaterialItemDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 主物料ID
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 初始数量
        /// </summary>
        public decimal InitQty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }

    /// <summary>
    /// 保存对象（物料加载）
    /// </summary>
    public class ManuFeedingMaterialSaveDto : CoreBaseDto
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 物料加载来源
        /// </summary>
        public ManuSFCFeedingSourceEnum? Source { get; set; }

        /// <summary>
        /// 上料点ID
        /// </summary>
        public long? FeedingPointId { get; set; }

        /// <summary>
        /// 主物料ID/产品ID（选中的主物料）
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 全部主物料ID集合
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }

    }

    /// <summary>
    /// 保存响应对象（物料加载）
    /// </summary>
    public record ManuFeedingMaterialResponseDto
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 主物料ID/产品ID（选中的主物料）
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 主物料编码/产品编码（选中的主物料）
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }

    #region 顷刻

    /// <summary>
    /// 上料转移
    /// </summary>
    public record ManuFeedingTransferSaveDto
    {
        /// <summary>
        /// 转移类型
        /// </summary>
        public ManuSFCFeedingTransferEnum TransferType { get; set; }

        /// <summary>
        /// 源id
        /// </summary>
        public long SourceId { get; set; }

        /// <summary>
        /// 目标id
        /// </summary>
        public long DestId { get; set; }

        /// <summary>
        /// 上料点类型的资源id
        /// </summary>
        public long LoadPointResoucesId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OpeationBy {  get; set; }
    }

    #endregion

}
