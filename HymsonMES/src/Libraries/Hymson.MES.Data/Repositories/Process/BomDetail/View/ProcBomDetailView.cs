﻿using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcBomDetailView: BaseEntity
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public string MaterialId { get; set; }

        /// <summary>
        /// 替代物料Id
        /// </summary>
        public string ReplaceMaterialId { get; set; }

        public long BomDetailId { get; set; }

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
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        ///    损耗
        /// </summary>
        public decimal? Loss { get; set; }

        /// <summary>
        /// 参考点
        /// </summary>
        public string ReferencePoint { get; set; }

        //工序id
        public string ProcedureId { get; set; }

        /// <summary>
        /// 工序代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 是否主物料，1：主物料
        /// </summary>
        public int IsMain { get; set; }
    }
}
