using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    public class InProductDismantleDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long BomDetailId { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

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
        /// 物料消耗信息
        /// </summary>
        public List<ManuSfcCirculation> Children { get; set; }
    }

    public class ManuSfcCirculation
    {
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
        /// 资源
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 状态,活动、移除、全部
        /// </summary>
        public InProductDismantleTypeEnum Status { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public  string UpdatedBy { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UpdatedOn { get; set; }
    }

    public class InProductDismantleQueryDto
    {
        public long BomId { get; set; }

        public InProductDismantleTypeEnum Type { get; set; }
    }
}
