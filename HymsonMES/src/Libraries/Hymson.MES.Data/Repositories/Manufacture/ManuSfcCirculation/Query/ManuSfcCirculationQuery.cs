using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码流转表 查询参数
    /// </summary>
    public class ManuSfcCirculationQuery
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
        /// 流转类型
        /// </summary>
        public IEnumerable<SfcCirculationTypeEnum> CirculationTypes { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 流转后主物料id
        /// </summary>
        public long? CirculationMainProductId { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }
    }

    /// <summary>
    /// 条码流转表 查询参数
    /// </summary>
    public class ManuSFCsCirculationQuery
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> SFCs { get; set; } = new List<string>();

        /// <summary>
        /// 流转类型
        /// </summary>
        public SfcCirculationTypeEnum[] CirculationTypes { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 流转后主物料id
        /// </summary>
        public long? CirculationMainProductId { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }
    }


    /// <summary>
    /// 条码流转表 查询参数
    /// </summary>
    public class ManuSfcCirculationBySfcsQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }


        /// <summary>
        /// 产品条码
        /// </summary>
        public IEnumerable<string> Sfc { get; set; }

        /// <summary>
        /// 转换后条码
        /// </summary>
        public string CirculationBarCode { get; set; }


        /// <summary>
        /// 转换后集合
        /// </summary>
        public IEnumerable<string> CirculationBarCodes { get; set; }

        /// <summary>
        /// 流转类型
        /// </summary>
        public SfcCirculationTypeEnum[] CirculationTypes { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 流转后主物料id
        /// </summary>
        public long? CirculationMainProductId { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }
    }
}
