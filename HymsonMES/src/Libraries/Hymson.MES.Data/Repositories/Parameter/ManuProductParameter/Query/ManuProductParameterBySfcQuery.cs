namespace Hymson.MES.Data.Repositories.Parameter
{
    public  class ManuProductParameterBySfcQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码属性
        /// </summary>
        public  IEnumerable<string> SFCs { get; set; }
    }
}
