/*
 *creator: Karl
 *
 *describe: 标准参数关联类型表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 05:06:17
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
    /// 标准参数关联类型表Dto
    /// </summary>
    public record ProcParameterLinkTypeDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 标准参数ID
        /// </summary>
        public long ParameterID { get; set; }

       /// <summary>
        /// 参数类型
        /// </summary>
        public int ParameterType { get; set; }

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
    /// 标准参数关联类型表新增Dto
    /// </summary>
    public record ProcParameterLinkTypeCreateDto 
    {
        /// <summary>
        /// 类型（设备/产品参数）
        /// </summary>
        public int ParameterType { get; set; } = 1;

        /// <summary>
        /// 集合（标准参数）
        /// </summary>
        public IEnumerable<long> Parameters { get; set; }
    }

    /// <summary>
    /// 标准参数关联类型表更新Dto
    /// </summary>
    public record ProcParameterLinkTypeModifyDto 
    {
        /// <summary>
        /// 类型（设备/产品参数）
        /// </summary>
        public int ParameterType { get; set; } = 1;

        /// <summary>
        /// 集合（标准参数）
        /// </summary>
        public IEnumerable<long> Parameters { get; set; }
    }

    /// <summary>
    /// 标准参数关联类型表分页Dto
    /// </summary>
    public class ProcParameterLinkTypePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 类型（设备/产品参数）
        /// </summary>
        public int ParameterType { get; set; } = 1;

        /// <summary>
        /// 编码（设备/产品参数）
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 名称（设备/产品参数）
        /// </summary>
        public string ParameterName { get; set; }
    }

    public record ProcParameterLinkTypeViewDto: BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 类型（设备/产品参数）
        /// </summary>
        public int ParameterType { get; set; } = 1;

        /// <summary>
        /// ID（标准参数）
        /// </summary>
        public long ParameterID { get; set; }

        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 查询对象（设备/产品参数） 分页
    /// </summary>
    public class ProcParameterDetailPagerQueryDto : PagerInfo 
    {
        /// <summary>
        /// 操作类型 1:add；2:edit；3:view；
        /// </summary>
        public string OperateType { get; set; } = "add";

        /// <summary>
        /// 类型（设备/产品参数）
        /// </summary>
        public int ParameterType { get; set; } = 1;

        /// <summary>
        /// 编码（设备/产品参数）
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 名称（设备/产品参数）
        /// </summary>
        public string ParameterName { get; set; }
    }
}
