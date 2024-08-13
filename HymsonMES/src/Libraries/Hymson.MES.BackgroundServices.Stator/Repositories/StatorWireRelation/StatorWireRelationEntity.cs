namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 定子铜线关系表
    /// </summary>
    public class StatorWireRelationEntity
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 内定子Id
        /// </summary>
        public long InnerId { get; set; }

        /// <summary>
        /// 铜线Id
        /// </summary>
        public long WireId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

    }
}
