/*
 *creator: Karl
 *
 *describe: 工序配置作业表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 02:23:23
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
    /// 工序配置作业表Dto
    /// </summary>
    public record ProcProcedureJobReleationDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 关联点(字典配置key值)
        /// </summary>
        public string LinkPoint { get; set; }

       /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

       /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsUse { get; set; }

       /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

       /// <summary>
        /// 备注
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
    /// 工序配置作业表新增Dto
    /// </summary>
    public record ProcProcedureJobReleationCreateDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 关联点(字典配置key值)
        /// </summary>
        public string LinkPoint { get; set; }

       /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

       /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsUse { get; set; }

       /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

       /// <summary>
        /// 备注
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
    /// 工序配置作业表更新Dto
    /// </summary>
    public record ProcProcedureJobReleationModifyDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 关联点(字典配置key值)
        /// </summary>
        public string LinkPoint { get; set; }

       /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

       /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsUse { get; set; }

       /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

       /// <summary>
        /// 备注
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
    /// 工序配置工作查询实体类
    /// </summary>
    public class QueryProcedureJobReleationDto
    {
        /// <summary>
        /// 工序配置工作实体类
        /// </summary>
        public ProcProcedureJobReleationDto ProcedureBomConfigJob { get; set; }

        /// <summary>
        /// 工作
        /// </summary>
        //public InteJobDto Job { get; set; }
    }

    /// <summary>
    /// 工序配置作业表分页Dto
    /// </summary>
    public class ProcProcedureJobReleationPagedQueryDto : PagerInfo
    {
        public long ProcedureId { get; set; }
    }
}
