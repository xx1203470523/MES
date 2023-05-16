namespace Hymson.MES.EquipmentServices.Request.BindSFC
{
    /// <summary>
    /// 条码绑定请求
    /// </summary>
    public class BindSFCRequest : BaseRequest
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 绑定的电芯条码列表
        /// </summary>
        public string[] BindSFCs { get; set; } = Array.Empty<string>();
    }
}
