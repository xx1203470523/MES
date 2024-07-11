namespace Hymson.MES.BackgroundServices.NIO.Dtos
{
    /// <summary>
    /// 基类
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// 是否已投产, true/false
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// 更改的时间, Unix 时间戳, 以秒为单位
        /// </summary>
        public long UpdateTime { get; set; }

    }

    /// <summary>
    /// 基类（请求）
    /// </summary>
    public class ClientRequestDto<T>
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

    /// <summary>
    /// 基类（响应）
    /// </summary>
    public class ClientResponseDto
    {

    }

}
