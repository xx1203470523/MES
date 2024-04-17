/*
 *creator: Karl
 *
 *describe: 上料点表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
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
    /// 上料点表Dto
    /// </summary>
    public record ProcLoadPointDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 上料点
        /// </summary>
        public string LoadPoint { get; set; }

       /// <summary>
        /// 上料点名称
        /// </summary>
        public string LoadPointName { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

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
        public string? UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 上料点表详情Dto
    /// </summary>
    public record ProcLoadPointDetailDto : BaseEntityDto
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 上料点
        /// </summary>
        public string LoadPoint { get; set; }

        /// <summary>
        /// 上料点名称
        /// </summary>
        public string LoadPointName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

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
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }

        public List<ProcLoadPointLinkMaterialViewDto>? LinkMaterials { get; set; }
        public List<ProcLoadPointLinkResourceViewDto>? LinkResources { get; set; }
    }


    /// <summary>
    /// 上料点表新增Dto
    /// </summary>
    public record ProcLoadPointCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 上料点
        /// </summary>
        public string LoadPoint { get; set; }

       /// <summary>
        /// 上料点名称
        /// </summary>
        public string LoadPointName { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        public List<ProcLoadPointLinkMaterialDto>? LinkMaterials { get; set; }
        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }
    }

    public record ImportLoadPointDto : BaseExcelDto
    {
        /// <summary>
        /// 上料点
        /// </summary>
        [EpplusTableColumn(Header = "上料点(必填)", Order = 1)]
        public string LoadPoint { get; set; }

        /// <summary>
        /// 上料点名称
        /// </summary>
        [EpplusTableColumn(Header = "上料点名称(必填)", Order = 2)]
        public string LoadPointName { get; set; }

        /// <summary>
        /// Bom描述
        /// </summary>
        [EpplusTableColumn(Header = "上料点描述", Order = 3)]
        public string? Remark { get; set; }
    }

    public record ExportLoadPointDto : BaseExcelDto
    {
        /// <summary>
        /// 上料点
        /// </summary>
        [EpplusTableColumn(Header = "上料点", Order = 1)]
        public string LoadPoint { get; set; }

        /// <summary>
        /// 上料点名称
        /// </summary>
        [EpplusTableColumn(Header = "上料点名称", Order = 2)]
        public string LoadPointName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EpplusTableColumn(Header = "状态", Order = 3)]
        public string Status { get; set; }
    }

    /// <summary>
    /// 上料点表更新Dto
    /// </summary>
    public record ProcLoadPointModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 上料点
        /// </summary>
        public string LoadPoint { get; set; }

       /// <summary>
        /// 上料点名称
        /// </summary>
        public string LoadPointName { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }

        public List<ProcLoadPointLinkMaterialDto>? LinkMaterials { get; set; }
        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }

    }

    /// <summary>
    /// 上料点表分页Dto
    /// </summary>
    public class ProcLoadPointPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :上料点 
        /// </summary>
        public string? LoadPoint { get; set; }

        /// <summary>
        /// 描述 :上料点名称 
        /// </summary>
        public string? LoadPointName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }

    public class LoadPointExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }
}
