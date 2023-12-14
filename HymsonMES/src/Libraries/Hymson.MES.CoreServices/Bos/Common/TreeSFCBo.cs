namespace Hymson.MES.CoreServices.Bos.Common
{
    /// <summary>
    /// 条码树
    /// </summary>
    public class TreeSFCBo
    {
        /// <summary>
        /// 条码ID
        /// </summary>
        public long SFCId { get; set; }

        /// <summary>
        /// 父条码ID
        /// </summary>
        public long ParentSFCId { get; set; }

    }
}
