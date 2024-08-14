namespace Hymson.MES.HttpClients.Options
{
    /// <summary>
    /// ERP操作配置
    /// </summary>
    public class ERPOptions
    {
        /// <summary>
        /// 基础地址
        /// </summary>
        public string BaseAddressUri { get; set; } = "";

        /// <summary>
        /// 请求token
        /// </summary>
        public string SysToken { get; set; } = "";

        /// <summary>
        /// 生产计划启用/关闭
        /// </summary>
        public string EnabledPlanRoute { get; set; } = "";

        /// <summary>
        /// NIO中需要的物料信息
        /// </summary>
        public string MaterialNioRoute { get; set; } = "";

    }

}
