/*
 *creator: Karl
 *
 *describe: 标准参数表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 02:50:20
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
    /// 标准参数表Dto
    /// </summary>
    public record ProcParameterDto : BaseEntityDto
    {
        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 参数代码
        /// </summary>
        public string ParameterCode { get; set; }

       /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

       /// <summary>
        /// 参数单位（字典定义）
        /// </summary>
        public string ParameterUnit { get; set; }

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
    /// 
    /// </summary>
    public record CustomProcParameterDto : ProcParameterDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
    }


    /// <summary>
    /// 标准参数表新增Dto
    /// </summary>
    public record ProcParameterCreateDto : BaseEntityDto
    {
        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数代码
        /// </summary>
        public string ParameterCode { get; set; } = "";

       /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; } = "";

        /// <summary>
        /// 参数单位（字典定义）
        /// </summary>
        public string ParameterUnit { get; set; } = "";

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 标准参数表更新Dto
    /// </summary>
    public record ProcParameterModifyDto : BaseEntityDto
    {
        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数代码
        /// </summary>
        public string ParameterCode { get; set; } = "";

       /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; } = "";

        /// <summary>
        /// 参数单位（字典定义）
        /// </summary>
        public string ParameterUnit { get; set; } = "";

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 标准参数表分页Dto
    /// </summary>
    public class ProcParameterPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 所属站点代码
        ///// </summary>
        //public long SiteId { get; set; }

        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        public string ParameterCode { get; set; } = "";

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        public string ParameterName { get; set; } = "";

        /// <summary>
        /// 描述（标准参数）
        /// </summary>
        public string Remark { get; set; } = "";
    }
}
