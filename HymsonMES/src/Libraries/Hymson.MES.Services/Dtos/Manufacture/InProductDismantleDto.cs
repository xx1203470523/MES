using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    public class InProductDismantleDto
    {
        /// <summary>
        /// Bom详情表id
        /// </summary>
        public long BomDetailId { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        /// 已装配数量
        /// </summary>
        public decimal AssembleCount { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

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
        public string Version { get; set; }

        /// <summary>
        /// 数据收集方式
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// bom描述信息
        /// </summary>
        public string BomRemark { get; set; }

        /// <summary>
        /// 组件信息
        /// </summary>
        public List<ManuSfcChildCirculationDto> Children { get; set; }
    }

    public class ManuSfcChildCirculationDto
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
        /// 工序
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 组件条码
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 流转条码数量
        /// </summary>
        public decimal CirculationQty { get; set; }

        /// <summary>
        /// 组件的产品描述信息
        /// </summary>
        public string MaterialRemark { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 状态,活动、移除、全部
        /// </summary>
        public SFCCirculationReportTypeEnum Status { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    public class InProductDismantleQueryDto
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// bomId
        /// </summary>
        public long BomId { get; set; }

        /// <summary>
        /// 查看类型
        /// </summary>
        public SFCCirculationReportTypeEnum Type { get; set; }
    }

    public class InProductDismantleRemoveDto
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 条码流转表主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public long ProcedureId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InProductDismantleAddDto
    {
        /// <summary>
        /// Bom详情表id
        /// </summary>
        public long BomDetailId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 外部时选择要上的物料
        /// </summary>
        public long? CirculationProductId { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 流转后主物料id
        /// </summary>
        public long? CirculationMainProductId { get; set; }

        /// <summary>
        /// 组件条码
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 是否组装
        /// </summary>
        public bool IsAssemble { get; set; } = true;

        /// <summary>
        /// 位置号
        /// </summary>
        public string? Location { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InProductDismantleReplaceDto : InProductDismantleAddDto
    {
        /// <summary>
        /// 被替换的旧的条码
        /// </summary>
        public string OldCirculationBarCode { get; set; }

        /// <summary>
        /// 被替换的数据id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 选择的物料数据收集方式
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }
    }

    public class CirculationQueryDto
    {
        /// <summary>
        /// 组件条码
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }
    }

    public class BarCodeQueryDto
    {
        /// <summary>
        /// 组件条码
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        public string Sfc { get; set; }

        public long ProcedureId { get; set; }

        /// <summary>
        /// 查看类型
        /// </summary>
        public SFCCirculationReportTypeEnum Type { get; set; }
    }

    public class BarCodeDataCollectionWayQueryDto
    {
        public long ProductId { get; set; }
        public long BomDetailId { get; set; }
        public long CirculationMainProductId { get; set; }
        public string CirculationBarCode { get; set; }
    }

    /// <summary>
    /// 组件配置信息
    /// </summary>
    public class InProductDismantleComponentDto
    {
        /// <summary>
        /// 组件信息
        /// </summary>
        public List<InProductDismantleInfoDto> InProductDismantleInfoDtos { get; set; }

        /// <summary>
        /// 物料主信息
        /// </summary>
        public ManuProcMaterialViewDto ManuProcMaterial { get; set; }
    }

    public class InProductDismantleInfoDto
    {
        /// <summary>
        /// Bom详情表id
        /// </summary>
        public long BomDetailId { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        /// 已装配数量
        /// </summary>
        public decimal AssembleCount { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

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
        public string Version { get; set; }

        /// <summary>
        /// 数据收集方式
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// bom描述信息
        /// </summary>
        public string BomRemark { get; set; }

        /// <summary>
        /// 组件信息
        /// </summary>
        public List<ManuSfcChildCirculationDto> Children { get; set; }
    }


    /// <summary>
    /// 组件配置物料返回实体
    /// </summary>
    public class ManuProcMaterialViewDto
    {
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
        public string Version { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string Sfc { get; set; }

    }

    /// <summary>
    /// 条码关系存储id
    /// </summary>
    public class InProductDismantleBusinessContent
    {
        /// <summary>
        /// Bom明细Id
        /// </summary>
        public long BomMainMaterialId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long BomId { get; set; }
        
    }
}
