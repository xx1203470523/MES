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
        public decimal conversionFactor { get; set; }


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
        /// Remark
        /// 空值 : false  
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 物料编码
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 转换系数状态
        /// </summary>
        public DisableOrEnableEnum OpenStatus { get; set; }

        /// <summary>
        /// 关联的物料列表
        /// </summary>
        public List<ProcLoadPointLinkMaterialDto> LinkMaterials { get; set; }

        /// <summary>
        /// 关联的资源列表
        /// </summary>
        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }
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
        public DisableOrEnableEnum? OpenStatus { get; set; }
            
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
        public decimal ConversionFactor { get; set; }


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
        public DisableOrEnableEnum OpenStatus { get; set; }
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
        public decimal conversionFactor { get; set; }


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
        public string? remark { get; set; }

        /// <summary>
        /// 转换系数状态
        /// </summary>
        public DisableOrEnableEnum  OpenStatus { get; set; }
        public List<ProcLoadPointLinkMaterialDto> LinkMaterials { get; set; }

        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }

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
        public decimal conversionFactor { get; set; }


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
        public DisableOrEnableEnum OpenStatus { get; set; }
        public List<ProcLoadPointLinkMaterialDto> LinkMaterials { get; set; }

        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }
    }
}
