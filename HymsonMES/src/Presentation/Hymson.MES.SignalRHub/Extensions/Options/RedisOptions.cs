namespace Hymson.MES.SignalRHub.Extensions.Options
{
    /// <summary>
    /// Redis 配置
    /// </summary>
    public class RedisOptions
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Redis连接
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 键值前缀
        /// </summary>
        public string InstanceName { get; set; }
    }
}
