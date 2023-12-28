namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除） 查询参数
    /// </summary>
    public class ManuContainerPackQuery
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Id组
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 最外层容器条码id
        /// </summary>
        public long? OutermostContainerBarCodeId { get; set; }

        /// <summary>
        /// 最外层容器条码组
        /// </summary>
        public IEnumerable<long> OutermostContainerBarCodeIds { get; set; }

        /// <summary>
        /// 容器条码id
        /// </summary>
        public long? ContainerBarCodeId { get; set; }

        /// <summary>
        /// 容器条码id组
        /// </summary>
        public IEnumerable<long>? ContainerBarCodeIds { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? LadeBarCode { get; set; }

        /// <summary>
        /// 条码s
        /// </summary>
        public IEnumerable<string> LadeBarCodes { get; set; }

        /// <summary>
        /// 装载最小深度
        /// </summary>
        public int? DeepMin { get; set; }

        /// <summary>
        /// 装载最大深度
        /// </summary>
        public int? DeepMax { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }
}
