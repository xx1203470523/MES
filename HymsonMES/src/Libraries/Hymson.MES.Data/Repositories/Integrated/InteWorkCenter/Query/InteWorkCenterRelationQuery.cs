namespace Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query
{
    /// <summary>
    /// 工作中心关联下属中心表查询对象
    /// </summary>
    public class InteWorkCenterRelationQuery
    {
        /// <summary>
        /// 子工作中心IDs
        /// </summary>
        public IEnumerable<long>? SubWorkCenterIds { get; set; }

        ///// <summary>
        ///// 站点Id 
        ///// </summary>
        //public long? SiteId { get; set; }
    }
}
