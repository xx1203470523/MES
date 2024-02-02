namespace Hymson.MES.CoreServices.Dtos.Manufacture
{
    public class BarcodeDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { set; get; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { set; get; }
    }
}
