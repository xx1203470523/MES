/*
 *creator: Karl
 *
 *describe: 打印设置表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2024-02-13 09:06:05
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Web.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 打印设置表Dto
    /// </summary>
    public record ProcPrintSetupDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 描述：业务类型
        /// 空值 : false  
        /// </summary>
        public CodeRuleCodeTypeEnum BusinessType { get; set; }
        /// <summary>
        /// 资源/类
        /// </summary>
        public PrintSetupEnum Type { get; set; }

        /// <summary>
        /// 资源类型名称
        /// </summary>
        public string ResTypeName { get; set; }

        /// <summary>
        /// 打印机
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模板文件
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 描述 :物料编码
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述：份数
        /// 空值 : false  
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// 描述:程序名
        /// 空值 : true  
        /// </summary>
        public string? Program { get; set; }

        /// <summary>
        /// 描述 :版本
        /// 空值 : true  
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 描述 :资源编码
        /// 空值 : true  
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 备注
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 描述 :打印模板ID
        /// 空值 : false  
        /// </summary>
        public long LabelTemplateId { get; set; }

        /// <summary>
        /// 描述:资源ID
        /// 空值 : true  
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 描述:打印机ID
        /// 空值 : true  
        /// </summary>
        public long? PrintId { get; set; }

        /// <summary>
        /// 描述 :物料ID
        /// 空值 : false  
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 描述 :类
        /// 空值 : false  
        /// </summary>
        public string Class { get; set; }
    }

    /// <summary>
    /// 打印设置表分页查询Dto
    /// </summary>
    public class ProcPrintSetupPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public CodeRuleCodeTypeEnum? BusinessType { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 资源/类
        /// </summary>
        public PrintSetupEnum? Type { get; set; }

        /// <summary>
        /// 打印机名称
        /// </summary>
        public string? PrintName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

    }

    /// <summary>
    /// 分页查询返回实体 
    /// </summary>
    public record ProcPrintSetupViewDto: ProcMaterialDto
    {
        /// <summary>
        /// 描述：业务类型
        /// 空值 : false  
        /// </summary>
        public CodeRuleCodeTypeEnum BusinessType { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述 :物料ID
        /// 空值 : false  
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 描述 :资源/类
        /// 空值 : false  
        /// </summary>
        public PrintSetupEnum? Type { get; set; }

        /// <summary>
        /// 描述 :资源ID
        /// 空值 : false  
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 描述 :打印机配置ID
        /// 空值 : false  
        /// </summary>
        public long? PrintId { get; set; }

        /// <summary>
        /// 描述 :标签模板ID
        /// 空值 : false  
        /// </summary>
        public long? LabelTemplateId { get; set; }

        /// <summary>
        /// 描述 :资源类型ID
        /// 空值 : false  
        /// </summary>
        public long? ResTypeId { get; set; }

    }

    /// <summary>
    /// 打印设置表新增Dto
    /// </summary>
    public record AddPrintSetupDto : BaseEntityDto
    {
        /// <summary>
        /// 描述 :物料id proc_material 的Id
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }


        /// <summary>
        /// 配置类型 1-资源 2-类
        /// 空值 : false  
        /// </summary>
        public PrintSetupEnum Type { get; set; }

        /// <summary>
        /// 描述:资源id proc_resource的Id
        /// 空值 : true  
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 描述:程序名
        /// 空值 : true  
        /// </summary>
        public string? Program { get; set; }

        /// <summary>
        /// 描述：业务类型
        /// 空值 : false  
        /// </summary>
        public CodeRuleCodeTypeEnum BusinessType { get; set; }

        /// <summary>
        /// 描述 :打印机ID
        /// 空值 : true
        /// </summary>
        public long? PrintId { get; set; }

        /// <summary>
        /// 描述 :打印模板ID
        /// 空值 : false  
        /// </summary>
        public long LabelTemplateId { get; set; }

        /// <summary>
        /// 描述：份数
        /// 空值 : false  
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 描述：备注
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }
    }

    /// <summary>
    /// 打印设置表更新Dto
    /// </summary>
    public record ProcPrintSetupModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 描述 :主建
        /// 空值 : false  
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述：份数
        /// 空值 : false  
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// 描述：物料ID
        /// 空值 : false  
        /// </summary>
        public string MaterialId { get; set; }

        /// <summary>
        /// 工序名
        /// 空值 : false  
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 描述 :标签模板ID
        /// 空值 : false  
        /// </summary>
        public long LabelTemplateId { get; set; }

        /// <summary>
        /// 转换系数状态
        /// </summary>  
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 描述：业务类型
        /// 空值 : false  
        /// </summary>
        public CodeRuleCodeTypeEnum BusinessType { get; set; }

        /// <summary>
        /// 描述：配置类型 1-资源 2-类
        /// 空值 : false  
        /// </summary>
        public PrintSetupEnum Type { get; set; }

        /// <summary>
        /// 描述 :打印机ID
        /// 空值 : true
        /// </summary>
        public long? PrintId { get; set; }


        /// <summary>
        /// 描述 :类
        /// 空值 : true
        /// </summary>
        public string? Class { get; set; }

        /// <summary>
        /// 描述 :程序
        /// 空值 : true
        /// </summary>
        public string? Program { get; set; }
    }
}
