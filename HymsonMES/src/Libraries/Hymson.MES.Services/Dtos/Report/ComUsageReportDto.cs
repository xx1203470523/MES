﻿using Confluent.Kafka;
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Mysqlx.Crud;
using OfficeOpenXml.Attributes;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 组件使用报告 分页参数
    /// </summary>
    public class ComUsageReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 组件物料编码ID
        /// </summary>
        public long? CirculationProductId { get; set; }

        /// <summary>
        /// 计划开始时间  时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        ///// <summary>
        ///// 开始时间 
        ///// </summary>
        //public DateTime? CreatedOnS
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(this.CreatedOn))
        //        {
        //            var dateArr = this.CreatedOn.Split(',');
        //            return dateArr.Length > 0 ? Convert.ToDateTime(dateArr[0]) : null;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 结束时间
        ///// </summary>
        //public DateTime? CreatedOnE
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(this.CreatedOn))
        //        {
        //            var dateArr = this.CreatedOn.Split(',');
        //            return dateArr.Length > 1 ? Convert.ToDateTime(dateArr[1]) : null;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        /// <summary>
        /// 批次条码
        /// </summary>
        public string? CirculationBarCode { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long? CirculationMainSupplierId { get; set; }
    }

    public class ComUsageReportViewDto
    {
        /// <summary>
        /// 车间作业控制
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 物料/版本
        /// </summary>
        public string ProductCodeVersion {get;set;}

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 组件车间作业
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 组件/版本
        /// </summary>
        public string CirculationProductCodeVersion { get; set; }
    }

    /// <summary>
    /// 组件使用模板模型
    /// </summary>
    public record ComUsageReportExcelExportDto : BaseExcelDto
    {
        /// <summary>
        /// 车间作业控制
        /// </summary>
        [EpplusTableColumn(Header = "车间作业控制", Order = 1)]
        public string SFC { get; set; }


        /// <summary>
        /// 物料/版本
        /// </summary>
        [EpplusTableColumn(Header = "物料/版本", Order = 2)]
        public string ProductCodeVersion { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        [EpplusTableColumn(Header = "工单", Order = 3)]
        public string OrderCode { get; set; }

        /// <summary>
        /// 组件车间作业
        /// </summary>
        [EpplusTableColumn(Header = "组件车间作业", Order = 4)]
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 组件/版本
        /// </summary>
        [EpplusTableColumn(Header = "组件/版本", Order = 5)]
        public string CirculationProductCodeVersion { get; set; }
    }

    public class ComUsageExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }
}
