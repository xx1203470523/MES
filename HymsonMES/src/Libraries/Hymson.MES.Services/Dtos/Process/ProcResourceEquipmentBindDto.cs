/*
 *creator: Karl
 *
 *describe: 资源设备绑定表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 11:20:47
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 资源设备绑定表Dto
    /// </summary>
    public record ProcResourceEquipmentBindDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属资源ID
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 是否主设备
        /// </summary>
        public bool? IsMain { get; set; }

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

    public record ProcResourceEquipmentBindViewDto: ProcResourceEquipmentBindDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备描述
        /// </summary>
        public string EquipmentDesc { get; set; }
    }

    /// <summary>
    /// 资源设备绑定表新增Dto
    /// </summary>
    public record ProcResourceEquipmentBindCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 是否主设备
        /// </summary>
        public bool? IsMain { get; set; }
    }

    /// <summary>
    /// 资源设备绑定表更新Dto
    /// </summary>
    public record ProcResourceEquipmentBindModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 是否主设备
        /// </summary>
        public bool? IsMain { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
      // [Required(ErrorMessage = "操作类型不可为空")]
      //  [Range(1, 3)]
        public int OperationType { get; set; }
    }

    /// <summary>
    /// 资源设备绑定表分页Dto
    /// </summary>
    public class ProcResourceEquipmentBindPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }
}
