namespace Hymson.MES.EquipmentServices.Request.BindSFC
{
    /// <summary>
    /// 条码解绑请求
    /// </summary>
    public class UnBindSFCRequest : BaseRequest
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 解绑的电芯条码列表
        /// </summary>
        public string[] BindSFCs { get; set; } = Array.Empty<string>();
    }
}
