/*
 *creator: Karl
 *
 *describe: 供应商    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Warehouse
{
    /// <summary>
    /// 供应商Dto
    /// </summary>
    public record WhSupplierDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

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

    }

    /// <summary>
    /// 更改供应商Dto
    /// </summary>
    public record UpdateWhSupplierDto : BaseEntityDto  
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 供应商新增Dto
    /// </summary>
    public record WhSupplierCreateDto : BaseEntityDto
    {
        ///// <summary>
        ///// 主键id
        ///// </summary>
        //public long Id { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }

    /// <summary>
    /// 供应商更新Dto
    /// </summary>
    public record WhSupplierModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }

    /// <summary>
    /// 供应商分页Dto
    /// </summary>
    public class WhSupplierPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 描述 : 供应商编号
        /// 空值 : true  
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 描述 :供应商名称 
        /// 空值 : true  
        /// </summary>
        public string? Name { get; set; }
    }
}
