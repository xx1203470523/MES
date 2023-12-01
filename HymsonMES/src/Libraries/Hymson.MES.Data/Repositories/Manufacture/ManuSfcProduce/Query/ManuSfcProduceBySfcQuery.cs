using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除） 根据SFC查询参数
    /// </summary>
    public class ManuSfcProduceBySfcQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }


    /// <summary>
    /// 条码生产信息（物理删除） 根据SFCs查询参数
    /// </summary>
    public class ManuSfcProduceBySfcsQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string>   Sfcs { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }

    public class ManuSfcProduceByProcedureIdStatusQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum Status { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }
    }
}
