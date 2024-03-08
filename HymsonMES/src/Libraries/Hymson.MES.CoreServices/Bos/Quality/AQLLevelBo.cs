namespace Hymson.MES.CoreServices.Bos.Quality
{
    /// <summary>
    /// 业务类（AQL检验水平）
    /// </summary>
    public class AQLLevelBo
    {
        /// <summary>
        /// 检验标准
        /// </summary>
        public string Standard { get; set; }

        /// <summary>
        /// 批次最小数量
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// 批次最大数量
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// VII
        /// </summary>
        public string VII { get; set; }

        /// <summary>
        /// VI
        /// </summary>
        public string VI { get; set; }

        /// <summary>
        /// V
        /// </summary>
        public string V { get; set; }

        /// <summary>
        /// IV
        /// </summary>
        public string IV { get; set; }

        /// <summary>
        /// III
        /// </summary>
        public string III { get; set; }

        /// <summary>
        /// II
        /// </summary>
        public string II { get; set; }

        /// <summary>
        /// I
        /// </summary>
        public string I { get; set; }

    }

}
