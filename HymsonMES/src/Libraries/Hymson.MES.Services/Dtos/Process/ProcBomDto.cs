/*
 *creator: Karl
 *
 *describe: BOM表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
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
    /// BOM表Dto
    /// </summary>
    public record ProcBomDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// BOM
        /// </summary>
        public string BomCode { get; set; }

       /// <summary>
        /// BOM名称
        /// </summary>
        public string BomName { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 是否当前版本
        /// </summary>
        public bool? IsCurrentVersion { get; set; }

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
    /// BOM表新增Dto
    /// </summary>
    public record ProcBomCreateDto : BaseEntityDto
    {
       /// <summary>
        /// BOM
        /// </summary>
        public string BomCode { get; set; }

       /// <summary>
        /// BOM名称
        /// </summary>
        public string BomName { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 是否当前版本
        /// </summary>
        public bool? IsCurrentVersion { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 物料集合，包含替代物料
        /// </summary>
        public IEnumerable<ProcBomDetailDto> MaterialList { get; set; }
    }

    /// <summary>
    /// BOM表更新Dto
    /// </summary>
    public record ProcBomModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// BOM名称
        /// </summary>
        public string BomName { get; set; }

        public string Version { get; set; }

       /// <summary>
        /// 是否当前版本
        /// </summary>
        public bool? IsCurrentVersion { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 物料集合，包含替代物料
        /// </summary>
        public IEnumerable<ProcBomDetailDto> MaterialList { get; set; }
    }

    /// <summary>
    /// BOM表分页Dto
    /// </summary>
    public class ProcBomPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :BomName 
        /// 空值 : false  
        /// </summary>
        public string? BomCode { get; set; }

        /// <summary>
        /// 描述 :BomName名称 
        /// 空值 : false  
        /// </summary>
        public string? BomName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }
    }

    /// <summary>
    /// Bom配置物料信息查询对象
    /// </summary>
    public class ProcBomMaterialQueryDto : PagerInfo
    {
        /// <summary>
        /// 所属BomID
        /// </summary>
        public long BomId { get; set; }
    }

    /// <summary>
    /// Bom导入模板模型
    /// </summary>
    public record ImportBomDto : BaseExcelDto
    {
        /// <summary>
        /// Bom编码
        /// </summary>
        [EpplusTableColumn(Header = "Bom(必填)", Order = 1)]
        public string BomCode { get; set; }

        /// <summary>
        /// Bom名称
        /// </summary>
        [EpplusTableColumn(Header = "Bom名称(必填)", Order = 2)]
        public string BomName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EpplusTableColumn(Header = "版本(必填)", Order = 3)]
        public string Version { get; set; }

        /// <summary>
        /// 是否当前版本
        /// </summary>
        [EpplusTableColumn(Header = "是否为当前版本(是/否)", Order = 4)]
        public YesOrNoEnum IsCurrentVersion { get; set; }


        /// <summary>
        /// Bom描述
        /// </summary>
        [EpplusTableColumn(Header = "Bom描述", Order = 5)]
        public string? Remark { get; set; }
    }

    public record ExportBomDto : BaseExcelDto
    {
        /// <summary>
        /// Bom编码
        /// </summary>
        [EpplusTableColumn(Header = "Bom", Order = 1)]
        public string BomCode { get; set; }

        /// <summary>
        /// Bom名称
        /// </summary>
        [EpplusTableColumn(Header = "Bom名称", Order = 2)]
        public string BomName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [EpplusTableColumn(Header = "版本", Order = 3)]
        public string Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EpplusTableColumn(Header = "状态", Order = 4)]
        public string Status { get; set; }

    }


    public class BomExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }
    /// <summary>
    /// 领料BOMQuery
    /// </summary>
    public class PickBomQueryDto
    {
        public long BomId { get; set; }

        public long WorkId { get; set; }
    }

    /// <summary>
    /// 领料Query
    /// </summary>
    public class PickQueryDto
    {
        /// <summary>
        /// 成套领料数量
        /// </summary>
        public decimal InputQty { get; set; } = 0;

        public long WorkId { get; set; }
    }
}
