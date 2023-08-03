﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    public record ProcResourceDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述 :资源代码 
        /// 空值 : false  
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 描述 :资源名称 
        /// 空值 : false  
        /// </summary>
        public string ResName { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 描述 :所属资源类型ID 
        /// 空值 : false  
        /// </summary>
        public long ResTypeId { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 资源维护表查询对象
    /// </summary>
    public class ProcResourcePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 描述 :资源代码 
        /// 空值 : false  
        /// </summary>
        public string? ResCode { get; set; }

        /// <summary>
        /// 描述 :资源名称 
        /// 空值 : false  
        /// </summary>
        public string? ResName { get; set; }

        /// <summary>
        /// 资源类型编码
        /// </summary>
        public string? ResType { get; set; }

        /// <summary>
        /// 资源类型id
        /// </summary>
        public long? ResTypeId { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public int? Status { get; set; }
    }

    /// <summary>
    /// 资源维护表：根据工序查询资源列表
    /// </summary>
    public class ProcResourceProcedurePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }

    public record ProcResourceViewDto : ProcResourceDto
    {
        /// <summary>
        /// 资源
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResTypeName { get; set; }
    }

    /// <summary>
    /// 资源新增实体类
    /// </summary>
    public record ProcResourceCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 资源代码
        /// </summary>
        //[Required(ErrorMessage = "资源代码不能为空")]
        //[MaxLength(length: 60, ErrorMessage = "资源代码超长")]
        public string ResCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        //[Required(ErrorMessage = "资源名称不能为空")]
        //[MaxLength(length: 60, ErrorMessage = "资源名称超长")]
        public string ResName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        //[Required(ErrorMessage = "状态不能为空")]
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 所属资源类型
        /// </summary>
        //[Required(ErrorMessage = "所属资源类型ID不能为空")]
        public long? ResTypeId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
       // [MaxLength(length: 255, ErrorMessage = "工序名称超长")]
        public string Remark { get; set; }

        /// <summary>
        /// 资源关联打印机
        /// </summary>
        public List<ProcResourceConfigPrintCreateDto> PrintList { get; set; }

        /// <summary>
        /// 资源关联设备
        /// </summary>
        public List<ProcResourceEquipmentBindCreateDto> EquList { get; set; }

        /// <summary>
        /// 资源设置
        /// </summary>
        public List<ProcResourceConfigResCreateDto> ResList { get; set; }

        /// <summary>
        /// 工作
        /// </summary>
        public List<ProcResourceConfigJobCreateDto> JobList { get; set; }

        /// <summary>
        ///产出设置信息
        /// </summary>
        public List<ProcProductSetCreateDto> ProductSetList { get; set; }

        ///// <summary>
        ///// 自定义
        ///// </summary>
        //public List<ProcResourceCustomDto> customList { get; set; }
    }

    /// <summary>
    /// 资源修改实体类
    /// </summary>
    public record ProcResourceModifyDto : BaseEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
       // [Required(ErrorMessage = "id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        //[Required(ErrorMessage = "资源名称不能为空")]
        //[MaxLength(length: 60, ErrorMessage = "资源名称超长")]
        public string ResName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
       // [Required(ErrorMessage = "状态不能为空")]
        public int Status { get; set; }

        /// <summary>
        /// 所属资源类型
        /// </summary>
       // [Required(ErrorMessage = "所属资源类型ID不能为空")]
        public long ResTypeId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
      //  [MaxLength(length: 255, ErrorMessage = "工序名称超长")]
        public string Remark { get; set; }

        /// <summary>
        /// 资源关联打印机
        /// </summary>
        public List<ProcResourceConfigPrintModifyDto> PrintList { get; set; }

        /// <summary>
        /// 资源关联设备
        /// </summary>
        public List<ProcResourceEquipmentBindModifyDto> EquList { get; set; }

        /// <summary>
        /// 资源设置
        /// </summary>
        public List<ProcResourceConfigResModifyDto> ResList { get; set; }

        /// <summary>
        /// 工作
        /// </summary>
        public List<ProcResourceConfigJobModifyDto> JobList { get; set; }

        /// <summary>
        ///产出设置信息
        /// </summary>
        public List<ProcProductSetCreateDto> ProductSetList { get; set; }

        ///// <summary>
        ///// 自定义
        ///// </summary>
        //public List<ProcResourceCustomDto> customList { get; set; }
    }
}
