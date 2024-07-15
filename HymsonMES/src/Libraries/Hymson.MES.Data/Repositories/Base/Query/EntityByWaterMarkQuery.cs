namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 水位查询实体
    /// </summary>
    public partial class EntityByWaterMarkQuery
    {
        /// <summary>
        /// 水位id
        /// </summary>
        public long StartWaterMarkId { set; get; }

        /// <summary>
        /// 条数
        /// </summary>
        public int Rows { set; get; }

    }
}
