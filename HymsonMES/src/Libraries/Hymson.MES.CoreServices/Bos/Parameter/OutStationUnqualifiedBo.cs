namespace Hymson.MES.CoreServices.Bos.Parameter
{
    /// <summary>
    /// 
    /// </summary>
    public record OutStationUnqualifiedBo
    {
        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }
        /// <summary>
        /// 不合格代码
        /// </summary>
        public decimal? UnqualifiedQty { get; set; }
    }
}
