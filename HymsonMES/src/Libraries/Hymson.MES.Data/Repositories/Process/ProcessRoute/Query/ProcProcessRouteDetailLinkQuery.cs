namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线工序节点关系明细表(前节点多条就存多条) 查询参数
    /// </summary>
    public class ProcProcessRouteDetailLinkQuery
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long? ProcessRouteId { get; set; }


        /// <summary>
        /// 所属工艺路线ID
        /// </summary>
        public IEnumerable<long>? ProcessRouteIds { get; set; }

        /// <summary>
        /// 工序ID（前工序ID）
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 所属工艺路线ID
        /// </summary>
        public IEnumerable<long>? ProcedureIds { get; set; }




        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }


        /// <summary>
        /// 序号
        /// </summary>
        public string? SerialNo { get; set; }

        /// <summary>
        /// 序号模糊条件
        /// </summary>
        public string? SerialNoLike { get; set; }


        /// <summary>
        /// 表主键ID（前节点）
        /// </summary>
        public long? PreProcessRouteDetailId { get; set; }

        /// <summary>
        /// 表主键ID（前节点）组
        /// </summary>
        public IEnumerable<long>? PreProcessRouteDetailIds { get; set; }


        /// <summary>
        /// 表主键ID（后节点）
        /// </summary>
        public long? ProcessRouteDetailId { get; set; }

        /// <summary>
        /// 表主键ID（后节点）组
        /// </summary>
        public IEnumerable<long>? ProcessRouteDetailIds { get; set; }


        /// <summary>
        /// 扩展字段1(暂存坐标)
        /// </summary>
        public string? Extra1 { get; set; }

        /// <summary>
        /// 扩展字段1(暂存坐标)模糊条件
        /// </summary>
        public string? Extra1Like { get; set; }


        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 说明模糊条件
        /// </summary>
        public string? RemarkLike { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 创建人模糊条件
        /// </summary>
        public string? CreatedByLike { get; set; }


        /// <summary>
        /// 创建时间开始日期
        /// </summary>
        public DateTime? CreatedOnStart { get; set; }

        /// <summary>
        /// 创建时间结束日期
        /// </summary>
        public DateTime? CreatedOnEnd { get; set; }


        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 最后修改人模糊条件
        /// </summary>
        public string? UpdatedByLike { get; set; }


        /// <summary>
        /// 修改时间开始日期
        /// </summary>
        public DateTime? UpdatedOnStart { get; set; }

        /// <summary>
        /// 修改时间结束日期
        /// </summary>
        public DateTime? UpdatedOnEnd { get; set; }


        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 站点Id组
        /// </summary>
        public IEnumerable<long>? SiteIds { get; set; }

    }
}
