/*
 *creator: Karl
 *
 *describe: 物料供应商关系    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-27 02:30:48
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
    /// 物料供应商关系Dto
    /// </summary>
    public record ProcMaterialSupplierRelationDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       
    }


    /// <summary>
    /// 物料供应商关系新增Dto
    /// </summary>
    public record ProcMaterialSupplierRelationCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

    }

    /// <summary>
    /// 物料供应商关系更新Dto
    /// </summary>
    public record ProcMaterialSupplierRelationModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       

    }

    /// <summary>
    /// 物料供应商关系分页Dto
    /// </summary>
    public class ProcMaterialSupplierRelationPagedQueryDto : PagerInfo
    {
        
    }
}
