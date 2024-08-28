namespace Hymson.MES.Data.Repositories.NioPushCollection.Query
{
    /// <summary>
    /// NIO推送参数 查询参数
    /// </summary>
    public class NioPushCollectionQuery
    {
        /// <summary>
        /// 水位ID
        /// </summary>
        public long WaterId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 重复数据查询
    /// </summary>
    public class NioPushCollectionRepeatQuery
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 工序列表
        /// </summary>
        public List<string> ProcedureList { get; set; } = new List<string>();
    }

    /// <summary>
    /// 重复数据查询
    /// </summary>
    public class NioPushCollectionSfcQuery
    {
        /// <summary>
        /// 条码
        /// </summary>
        public List<string> SfcList { get; set; } = new List<string>();

        /// <summary>
        /// 工序列表
        /// </summary>
        public List<string> ProcedureList { get; set; } = new List<string>();
    }
}
