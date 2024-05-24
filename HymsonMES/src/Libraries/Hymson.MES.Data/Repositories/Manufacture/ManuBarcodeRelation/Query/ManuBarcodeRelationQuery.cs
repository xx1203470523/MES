using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;
using Org.BouncyCastle.Cms;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 条码关系表 查询参数
    /// </summary>
    public class ManuBarcodeRelationQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 投入条码
        /// </summary>
        public IEnumerable<string> InputBarCodes { get; set; } 

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }
        /// <summary>
        /// 产出步骤表
        /// </summary>
        public long? OutputSfcStepId { get; set; }

        /// <summary>
        /// 投入条码表
        /// </summary>
        public long? InputSfcStepId { get; set; }


        /// <summary>
        /// 投入条码表
        /// </summary>
        public long? DisassembledSfcStepId { get; set; }
    }

    /// <summary>
    /// 组件配置表 查询参数
    /// </summary>
    public class ManuComponentBarcodeRelationQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public SFCCirculationReportTypeEnum IsDisassemble { get; set; }

        /// <summary>
        /// Bom明细表ID
        /// </summary>

        public string? BomMainMaterialId { get; set; }

        /// <summary>
        /// 产出条码
        /// </summary>
        public string InputBarCode { get; set; }
    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class ManuComponentBarcodeRelationLocationQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 条码（产品序列码）
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Location { get; set; }

    }
}
