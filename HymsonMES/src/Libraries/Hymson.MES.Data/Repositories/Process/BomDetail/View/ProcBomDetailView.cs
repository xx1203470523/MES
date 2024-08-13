using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM明细
    /// </summary>
    public class ProcBomDetailView : BaseEntity
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 替代物料Id
        /// </summary>
        public long ReplaceMaterialId { get; set; }

        /// <summary>
        /// BOM明细ID
        /// </summary>
        public long BomDetailId { get; set; }

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
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        /// 损耗
        /// </summary>
        public decimal? Loss { get; set; }

        /// <summary>
        /// 参考点
        /// </summary>
        public string ReferencePoint { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }


        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }

        /// <summary>
        /// 是否启用替代物料
        /// </summary>
        public bool IsEnableReplace { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }


        /// <summary>
        /// 工序代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 是否主物料，1：主物料
        /// </summary>
        public int IsMain { get; set; }

        /// <summary>
        /// Bom类型
        /// </summary>

        public ManuProductTypeEnum BomProductType { get; set; }

    }

    /// <summary>
    /// BOM明细
    /// </summary>
    public class ProcOrderBomDetailDto : BaseEntity
    {
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
        public string Version { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        /// 批次大小
        /// </summary>
        public decimal Batch { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit {  get; set; }
    }
}
