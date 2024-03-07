using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 收货单物料分页查询Dto
    /// </summary>
    public class ReceiptMaterialPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 收货单号
        /// </summary>
        public string? ReceiptNum { get; set; }

        /// <summary>
        /// 编码（物料）
        /// </summary>
        public string? SupplierCode { get; set; }

        /// <summary>
        /// 名称（物料）
        /// </summary>
        public string? SupplierName { get; set; }

    }

    /// <summary>
    /// 收货物料
    /// </summary>
    public record ReceiptMaterialDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 收货物料
    /// </summary>
    public record ReceiptMaterialItemDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string SupplierBatch { get; set; }

        /// <summary>
        /// 物料Id
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
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 内部
        /// </summary>
        public string InternalBatch { get; set; }

        /// <summary>
        /// 计划发货数量
        /// </summary>
        public int PlanQty { get; set; }

        /// <summary>
        /// 计划到货时间
        /// </summary>
        public DateTime? PlanTime { get; set; }

    }

    /// <summary>
    /// 生成检验单Dto
    /// </summary>
    public record GenerateInspectionDto
    {
        /// <summary>
        /// 收货单
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 收货单明细ID集合
        /// </summary>
        public IEnumerable<long> Details { get; set; }

    }

    /// <summary>
    /// 检验参数Dto
    /// </summary>
    public record InspectionParameterDetailDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数Id proc_parameter 的id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数数据类型
        /// </summary>
        public int ParameterDataType { get; set; }

        /// <summary>
        /// 检验器具
        /// </summary>
        public int Utensil { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal UpperLimit { get; set; }

        /// <summary>
        /// 规格中心
        /// </summary>
        public decimal Center { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal LowerLimit { get; set; }

        /// <summary>
        /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
        /// </summary>
        public int InspectionType { get; set; }

    }

}
