/*
 *creator: Karl
 *
 *describe: 转换系数表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 转换系数表Dto
    /// </summary>
    public record ProcConversionFactorDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述 :转换系数
        /// 空值 : false  
        /// </summary>
        public string conversionFactor { get; set; }


        /// <summary>
        /// 描述 :工序ID
        /// 空值 : false  
        /// </summary>
        public long procedureId { get; set; }

        /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序编码
        /// 空值 : false  
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 工序名
        /// 空值 : false  
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 物料名
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 转换系数状态
        /// </summary>
        public ManuSfcRepairDetailIsIsCloseEnum OpenStatus { get; set; }

        /// <summary>
        /// 关联的物料列表
        /// </summary>
        public List<ProcLoadPointLinkMaterialDto> LinkMaterials { get; set; }

        /// <summary>
        /// 关联的资源列表
        /// </summary>
        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }
    }

    public record ProcConversionFactorCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 工序BOM代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序BOM名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        public ProcedureTypeEnum Type { get; set; }

        /// <summary>
        /// 包装等级（字典数据）
        /// </summary>
        public int? PackingLevel { get; set; }

        /// <summary>
        /// 所属资源类型ID
        /// </summary>
        public long? ResourceTypeId { get; set; }

        /// <summary>
        /// 循环次数
        /// </summary>
        public int? Cycle { get; set; }

        /// <summary>
        /// 是否维修返回
        /// </summary>
        public bool? IsRepairReturn { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 是否复判
        /// </summary>
        public bool IsRejudges { get; set; }

        /// <summary>
        /// 设备NG信息校验
        /// </summary>
        public bool IsValidNGCodes { get; set; }

        /// <summary>
        /// 标记不合格id
        /// </summary>
        public long? MarkId { get; set; }

        /// <summary>
        /// 缺陷不合格ID
        /// </summary>
        public long? DefectId { get; set; }

        /// <summary>
        /// 是否复判
        /// </summary>
        public TrueOrFalseEnum? IsRejudge { get; set; }

        /// <summary>
        /// 是否校验NG信息
        /// </summary>
        public TrueOrFalseEnum? IsValidNGCode { get; set; }

    }

    /// <summary>
    /// 转换系数表分页查询Dto
    /// </summary>
    public class ProcConversionFactorPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 物料名
        /// 空值 : false  
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// 空值 : false  
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ManuSfcRepairDetailIsIsCloseEnum? OpenStatus { get; set; }

    }

    /// <summary>
    /// 分页查询返回实体
    /// </summary>
    public record ProcConversionFactorViewDto : ProcProcedureDto
    {

        /// <summary>
        /// 描述 :转换系数
        /// 空值 : false  
        /// </summary>
        public string ConversionFactor { get; set; }


        /// <summary>
        /// 物料名
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public ManuSfcRepairDetailIsIsCloseEnum OpenStatus { get; set; }
    }

    public class QueryConversionFactorDto 
    {
        /// <summary>
        /// 描述 :转换系数
        /// 空值 : false  
        /// </summary>
        public string conversionFactor { get; set; }


        /// <summary>
        /// 描述 :工序ID
        /// 空值 : false  
        /// </summary>
        public long procedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// 空值 : false  
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 工序名
        /// 空值 : false  
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 物料名
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 转换系数状态
        /// </summary>
        public ManuSfcRepairDetailIsIsCloseEnum OpenStatus { get; set; }
        public List<ProcLoadPointLinkMaterialDto> LinkMaterials { get; set; }

        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }

    }

    /// <summary>
    /// 转换系数表新增Dto
    /// </summary>
    public record AddProcConversionFactorDto : BaseEntityDto
    {
        /// <summary>
        /// 工序信息
        /// </summary>
        public ProcProcedureCreateDto Procedure { get; set; }

        /// <summary>
        /// 工序打印配置信息
        /// </summary>

        public List<ProcProcedurePrintReleationCreateDto> ProcedurePrintList { get; set; }

        /// <summary>
        /// 工序工作配置信息
        /// </summary>
        public List<InteJobBusinessRelationCreateDto> ProcedureJobList { get; set; }

        /// <summary>
        ///产出设置信息
        /// </summary>
        public List<ProcProductSetCreateDto> ProductSetList { get; set; }
    }

    /// <summary>
    /// 转换系数表新增Dto
    /// </summary>
    public record AddConversionFactorDto : BaseEntityDto
    {
        /// <summary>
        /// 描述 :转换系数
        /// 空值 : false  
        /// </summary>
        public string conversionFactor { get; set; }


        /// <summary>
        /// 描述 :工序ID
        /// 空值 : false  
        /// </summary>
        public long procedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// 空值 : false  
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 工序名
        /// 空值 : false  
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 转换系数状态
        /// </summary>
        public ManuSfcRepairDetailIsIsCloseEnum  OpenStatus { get; set; }
        public List<ProcLoadPointLinkMaterialDto> LinkMaterials { get; set; }

        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }

    }

    /// <summary>
    /// 转换系数表修改Dto
    /// </summary>
    public record UpdateProcConversionFactorDto : BaseEntityDto
    {
        /// <summary>
        /// 工序信息
        /// </summary>
        public ProcProcedureModifyDto Procedure { get; set; }

        /// <summary>
        /// 工序打印配置信息
        /// </summary>

        public List<ProcProcedurePrintReleationCreateDto> ProcedurePrintList { get; set; }

        /// <summary>
        /// 工序工作配置信息
        /// </summary>
        public List<InteJobBusinessRelationCreateDto> ProcedureJobList { get; set; }

        /// <summary>
        ///产出设置信息
        /// </summary>
        public List<ProcProductSetCreateDto> ProductSetList { get; set; }
    }

    /// <summary>
    /// 转换系数表更新Dto
    /// </summary>
    public record ProcConversionFactorModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 描述 :主建
        /// 空值 : false  
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 描述 :转换系数
        /// 空值 : false  
        /// </summary>
        public string conversionFactor { get; set; }


        /// <summary>
        /// 描述 :工序ID
        /// 空值 : false  
        /// </summary>
        public long procedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// 空值 : false  
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 工序名
        /// 空值 : false  
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 工序名
        /// 空值 : false  
        /// </summary>
        public string? Remark { get; set; }


        /// <summary>
        /// 转换系数状态
        /// </summary>
        public ManuSfcRepairDetailIsIsCloseEnum OpenStatus { get; set; }
        public List<ProcLoadPointLinkMaterialDto> LinkMaterials { get; set; }

        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }
    }

    /// <summary>
    /// 工序配置打印查询实体类
    /// </summary>
    public class ConversionFactorJobReleationDto
    {
        /// <summary>
        /// 关联点
        /// </summary>
        public int LinkPoint { get; set; } = -1;

        /// <summary>
        /// 所属不合格代码ID
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// 作业编号 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 作业名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 作业描述
        /// </summary>
        public string Remark { get; set; } = "";
    }

    public class ProcConversionFactorCodeDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工序BOM代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序BOM名称
        /// </summary>
        public string Name { get; set; }
    }
}
