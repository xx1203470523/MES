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

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 工具管理表Dto
    /// </summary>
    public record EquToolingManageDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述 :工具管理
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
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 工具管理状态
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
    /// 工具管理表分页查询Dto
    /// </summary>
    public class EquToolingManagePagedQueryDto : PagerInfo
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
        /// 工具类型编码
        /// 空值 : false  
        /// </summary>
        public string? ToolsTypeCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 更新时间  时间范围  数组
        /// </summary>
        public DateTime[]? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 分页查询返回实体
    /// </summary>
    public record EquToolingManageViewDto : BaseEntityDto
    {

        /// <summary>
        /// 工具类型编码
        /// 空值 : false  
        /// </summary>
        public string ToolsTypeCode { get; set; }

        /// <summary>
        /// 工具类型名称
        /// 空值 : false  
        /// </summary>
        public string ToolsTypeName { get; set; }

        /// <summary>
        /// 工具编码
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工具名称
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 额定使用寿命
        /// </summary>
        public decimal? RatedLife { get; set; }

        /// <summary>
        /// 是否效准
        /// </summary>
        public YesOrNoEnum? IsCalibrated { get; set; }

        /// <summary>
        /// 校准周期
        /// </summary>
        public decimal? CalibrationCycle { get; set; }

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
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 工具管理表主键
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// 工具管理表新增Dto
    /// </summary>
    public record AddEquToolingManageDto : BaseEntityDto
    {
        /// <summary>
        /// 描述 :工具管理
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
        /// 工具管理状态
        /// </summary>
        public DisableOrEnableEnum OpenStatus { get; set; }
        /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }

    }

    /// <summary>
    /// 工具管理表更新Dto
    /// </summary>
    public record EquToolingManageModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 描述 :主建
        /// 空值 : false  
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 描述 :工具管理
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
        /// 工具管理状态
        /// </summary>  
        public DisableOrEnableEnum OpenStatus { get; set; }
        public List<ProcLoadPointLinkMaterialDto> LinkMaterials { get; set; }

        public List<ProcLoadPointLinkResourceDto>? LinkResources { get; set; }
    }
}
