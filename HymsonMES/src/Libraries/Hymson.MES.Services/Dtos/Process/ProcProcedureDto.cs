/*
 *creator: Karl
 *
 *describe: 工序表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 工序表Dto
    /// </summary>
    public record ProcProcedureDto : BaseEntityDto
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

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ProcedureTypeEnum Type { get; set; }

        /// <summary>
        /// 包装等级（字典数据）
        /// </summary>
        public int PackingLevel { get; set; }

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
        public string Version { get; set; }

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

    public record ProcProcedureCreateDto : BaseEntityDto
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
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

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
    }

    /// <summary>
    /// 工序表分页查询Dto
    /// </summary>
    public class ProcProcedurePagedQueryDto : PagerInfo
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
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 状态列表
        /// </summary>
        public SysDataStatusEnum[]? StatusArr { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ProcedureTypeEnum? Type { get; set; }

        /// <summary>
        /// 类型列表
        /// </summary>
        public ProcedureTypeEnum[]? TypeArr { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// </summary>
        public string? ResTypeName { get; set; }
    }

    /// <summary>
    /// 分页查询返回实体
    /// </summary>
    public record ProcProcedureViewDto : ProcProcedureDto
    {
        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 资源类型名称
        /// </summary>
        public string ResTypeName { get; set; }
    }

    public class QueryProcProcedureDto
    {
        /// <summary>
        /// 工序信息
        /// @author wangkeming
        /// @date 2022-09-30
        /// </summary>
        public ProcProcedureDto Procedure { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public ProcResourceTypeDto ResourceType { get; set; }
    }

    /// <summary>
    /// 工序表新增Dto
    /// </summary>
    public record AddProcProcedureDto : BaseEntityDto
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
    }

    /// <summary>
    /// 工序表修改Dto
    /// </summary>
    public record UpdateProcProcedureDto : BaseEntityDto
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
    }

    /// <summary>
    /// 工序表更新Dto
    /// </summary>
    public record ProcProcedureModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public long Id { get; set; }

        ///// <summary>
        ///// 工序BOM代码
        ///// </summary>
        //public string Code { get; set; }

        /// <summary>
        /// 工序BOM名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

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
    }

    /// <summary>
    /// 工序配置打印查询实体类
    /// </summary>
    public class ProcedureJobReleationDto
    {
        /// <summary>
        /// 工序配置工作实体类
        /// </summary>
        // public InteJobBusinessRelationDto ProcedureConfigJob { get; set; }

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
        public string Remark { get; set; }
    }
}
