namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 水位查询实体
    /// </summary>
    public class EntityByWaterMarkTimeQuery
    {
        /// <summary>
        /// 水位时间
        /// </summary>
        public DateTime StartWaterMarkTime { set; get; }

        /// <summary>
        /// 条数
        /// </summary>
        public int Rows { set; get; }

    }
}
