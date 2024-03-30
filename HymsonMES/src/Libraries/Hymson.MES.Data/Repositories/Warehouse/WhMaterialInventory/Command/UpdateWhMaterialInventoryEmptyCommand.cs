namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateWhMaterialInventoryEmptyCommand
    {
        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> BarCodeList { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public  DateTime UpdateTime { get; set; }
    }
}
