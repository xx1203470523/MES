/*
 *creator: Karl
 *
 *describe: 物料组维护表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-10 03:54:07
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
    /// 物料组维护表Dto
    /// </summary>
    public record ProcMaterialGroupDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 物料组编号
        /// </summary>
        public string GroupCode { get; set; }

       /// <summary>
        /// 物料组名称
        /// </summary>
        public string GroupName { get; set; }

       /// <summary>
        /// 物料组版本
        /// </summary>
        public string GroupVersion { get; set; }

       /// <summary>
        /// 物料组描述
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
    /// 物料组维护表新增Dto
    /// </summary>
    public record ProcMaterialGroupCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 物料组编号
        /// </summary>
        public string GroupCode { get; set; }

       /// <summary>
        /// 物料组名称
        /// </summary>
        public string GroupName { get; set; }

       /// <summary>
        /// 物料组版本
        /// </summary>
        public string GroupVersion { get; set; }

       /// <summary>
        /// 物料组描述
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
    /// 物料组维护表更新Dto
    /// </summary>
    public record ProcMaterialGroupModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 物料组编号
        /// </summary>
        public string GroupCode { get; set; }

       /// <summary>
        /// 物料组名称
        /// </summary>
        public string GroupName { get; set; }

       /// <summary>
        /// 物料组版本
        /// </summary>
        public string GroupVersion { get; set; }

       /// <summary>
        /// 物料组描述
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
    /// 物料组维护表分页Dto
    /// </summary>
    public class ProcMaterialGroupPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; } = "";

        /// <summary>
        /// 物料组编码
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 物料组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }

    /// <summary>
    /// 物料组维护表自定义查询对象
    /// </summary>
    public class CustomProcMaterialGroupPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; } = "";

        /// <summary>
        /// 物料组编码
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }

    /// <summary>
    /// 自定义分组 视图
    /// </summary>
    public record CustomProcMaterialGroupViewDto : ProcMaterialDto
    {

        #region 物料相关属性
        /// <summary>
        /// 描述 :物料编码 
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述 :物料名称 
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 描述 :版本 
        /// 空值 : true  
        /// </summary>
        public string Version { get; set; }
        #endregion

    }

}
