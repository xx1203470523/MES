using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcEsopView: ProcEsopEntity
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

       /// <summary>
       /// 物料名称
       /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }
    }
}
