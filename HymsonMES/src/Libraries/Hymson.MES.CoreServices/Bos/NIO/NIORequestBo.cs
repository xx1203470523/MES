namespace Hymson.MES.CoreServices.Bos.NIO
{
    /// <summary>
    /// 基类（请求）
    /// </summary>
    public class NIORequestBo<T>
    {
        /// <summary>
        /// 业务场景编码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        public IEnumerable<T> List { get; set; }
    }


}
