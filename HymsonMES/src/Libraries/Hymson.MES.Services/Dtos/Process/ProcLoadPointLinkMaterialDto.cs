/*
 *creator: Karl
 *
 *describe: 上料点关联物料表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-18 09:31:10
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
    /// 上料点关联物料表Dto
    /// </summary>
    public record ProcLoadPointLinkMaterialDto : BaseEntityDto
    {
       /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 所属物料Code
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 所属物料Code
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

       /// <summary>
        /// 参考点
        /// </summary>
        public string? ReferencePoint { get; set; }

    }


    /// <summary>
    /// 上料点关联物料表新增Dto
    /// </summary>
    public record ProcLoadPointLinkMaterialCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 序号( 程序生成)
        /// </summary>
        public string SerialNo { get; set; }

       /// <summary>
        /// 所属上料点ID
        /// </summary>
        public long LoadPointId { get; set; }

       /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 参考点
        /// </summary>
        public string ReferencePoint { get; set; }

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
        public string? UpdatedBy { get; set; }

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
    /// 上料点关联物料表更新Dto
    /// </summary>
    public record ProcLoadPointLinkMaterialModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 序号( 程序生成)
        /// </summary>
        public string SerialNo { get; set; }

       /// <summary>
        /// 所属上料点ID
        /// </summary>
        public long LoadPointId { get; set; }

       /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 参考点
        /// </summary>
        public string ReferencePoint { get; set; }

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
        public string? UpdatedBy { get; set; }

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
    /// 上料点关联物料表分页Dto
    /// </summary>
    public class ProcLoadPointLinkMaterialPagedQueryDto : PagerInfo
    {
        
    }

    /// <summary>
    /// 上料点关联物料表Dto
    /// </summary>
    public record ProcLoadPointLinkMaterialViewDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述 :所属物料ID 
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 描述 :参考点 
        /// </summary>
        public string ReferencePoint { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }
    }

}
