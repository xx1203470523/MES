using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Report;

namespace Hymson.MES.Services.Dtos.Report
{
    public class OriginalSummaryQueryDto
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 查看类型
        /// </summary>
        public OriginalSummaryReportTypeEnum Type { get; set; }
    }

    public class OriginalSummaryReportDto
    {
        /// <summary>
        /// BomId
        /// </summary>
        public long BomId { get; set; }

        /// <summary>
        /// Bom详情表id
        /// </summary>
        public long BomDetailId { get; set; }

        /// <summary>
        /// 产品编码、版本
        /// </summary>
        public string ProductRemark { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// bom描述信息(编码/版本)
        /// </summary>
        public string BomRemark { get; set; }

        /// <summary>
        /// bom名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// Bom组件/版本
        /// </summary>
        public string CirculationBomRemark { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string CirculationName { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        /// 已装配数量
        /// </summary>
        public decimal AssembleCount { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组件信息
        /// </summary>
        public List<OriginalSummaryChildDto> Children { get; set; }
    }

    public class OriginalSummaryChildDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Bom详情表id
        /// </summary>
        public long BomDetailId { get; set; }

        /// <summary>
        /// 组件的产品描述信息
        /// </summary>
        public string CirculationRemark { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string CirculationName { get; set; }

        /// <summary>
        /// 资源,操作点
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 组件条码
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 流转条码数量
        /// </summary>
        public decimal CirculationQty { get; set; }

        /// <summary>
        /// 状态,活动、移除、全部
        /// </summary>
        public InProductDismantleTypeEnum Status { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
