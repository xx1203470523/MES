/*
 *creator: Karl
 *
 *describe: BOM明细表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:38:06
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using MimeKit;

using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// BOM明细表Dto
    /// </summary>
    public record ProcBomDetailDto : BaseEntityDto
    {
        public string MaterialId { get; set; }

        public string? ReplaceMaterialId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

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
        public string? ReferencePoint { get; set; }

        public string ProcedureId { get; set; }

        /// <summary>
        /// 工序代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否主物料，1：主物料
        /// </summary>
        public int IsMain { get; set; }



        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }

        /// <summary>
        /// 是否启用替代物料
        /// </summary>
        public bool? IsEnableReplace { get; set; } = false;

        /// <summary>
        /// 序号
        /// </summary>
        public decimal Seq { get; set; }


        /// <summary>
        /// Bom类型
        /// </summary>

        public ManuProductTypeEnum BomProductType { get; set; }
    }


    /// <summary>
    /// BOM明细表新增Dto
    /// </summary>
    public record ProcBomDetailCreateDto : BaseEntityDto
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
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

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
        public string Remark { get; set; } = "";

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


        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum DataCollectionWay { get; set; }

        /// <summary>
        /// 是否启用替代物料
        /// </summary>
        public bool IsEnableReplace { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }
    }

    /// <summary>
    /// BOM明细表更新Dto
    /// </summary>
    public record ProcBomDetailModifyDto : BaseEntityDto
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
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

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
        public string Remark { get; set; } = "";

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


        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum DataCollectionWay { get; set; }

        /// <summary>
        /// 是否启用替代物料
        /// </summary>
        public bool IsEnableReplace { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

    }

    /// <summary>
    /// BOM明细表分页Dto
    /// </summary>
    public class ProcBomDetailPagedQueryDto : PagerInfo
    {
        
    }

}
