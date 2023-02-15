/*
 *creator: Karl
 *
 *describe: BOM明细替代料表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 05:33:28
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// BOM明细替代料表Dto
    /// </summary>
    public record ProcBomDetailReplaceMaterialDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属BomID
        /// </summary>
        public long BomId { get; set; }

       /// <summary>
        /// 所属BOM明细表ID
        /// </summary>
        public long BomDetailId { get; set; }

       /// <summary>
        /// 所属BOM替代物料ID（从物料表选择）
        /// </summary>
        public long? ReplaceMaterialId { get; set; }

       /// <summary>
        /// 参考点
        /// </summary>
        public string ReferencePoint { get; set; }

       /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

       /// <summary>
        /// 损耗
        /// </summary>
        public decimal? Loss { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }

       
    }


    /// <summary>
    /// BOM明细替代料表新增Dto
    /// </summary>
    public record ProcBomDetailReplaceMaterialCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属BomID
        /// </summary>
        public long BomId { get; set; }

       /// <summary>
        /// 所属BOM明细表ID
        /// </summary>
        public long BomDetailId { get; set; }

       /// <summary>
        /// 所属BOM替代物料ID（从物料表选择）
        /// </summary>
        public long? ReplaceMaterialId { get; set; }

       /// <summary>
        /// 参考点
        /// </summary>
        public string ReferencePoint { get; set; }

       /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

       /// <summary>
        /// 损耗
        /// </summary>
        public decimal? Loss { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }

       
    }

    /// <summary>
    /// BOM明细替代料表更新Dto
    /// </summary>
    public record ProcBomDetailReplaceMaterialModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属BomID
        /// </summary>
        public long BomId { get; set; }

       /// <summary>
        /// 所属BOM明细表ID
        /// </summary>
        public long BomDetailId { get; set; }

       /// <summary>
        /// 所属BOM替代物料ID（从物料表选择）
        /// </summary>
        public long? ReplaceMaterialId { get; set; }

       /// <summary>
        /// 参考点
        /// </summary>
        public string ReferencePoint { get; set; }

       /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

       /// <summary>
        /// 损耗
        /// </summary>
        public decimal? Loss { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }

       

    }

    /// <summary>
    /// BOM明细替代料表分页Dto
    /// </summary>
    public class ProcBomDetailReplaceMaterialPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }
    }
}
