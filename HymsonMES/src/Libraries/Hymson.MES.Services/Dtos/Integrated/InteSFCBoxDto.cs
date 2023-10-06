using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Integrated
{
    public record InteSFCBoxDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 电芯条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 箱码
        /// </summary>
        public string BoxCode { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 状态;1、开启 2、恢复
        /// </summary>
        public SFCBoxEnum Status { get; set; }

        /// <summary>
        /// 传输时间
        /// </summary>
        public DateTime Localtime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// OCVB
        /// </summary>
        public decimal Ocvb { get; set; }

        /// <summary>
        /// OCVB数采时间
        /// </summary>
        public DateTime OcvbDate { get; set; }

        /// <summary>
        /// 二次注液注液后重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// DC容量
        /// </summary>
        public decimal Dc { get; set; }

        /// <summary>
        /// DC容量数采时间
        /// </summary>
        public DateTime DcDate { get; set; }

        /// <summary>
        /// IMPB
        /// </summary>
        public decimal Impb { get; set; }

        /// <summary>
        /// 自放电率X
        /// </summary>
        public decimal SelfDischargeRate { get; set; }

        /// <summary>
        /// 电芯宽度（包膜）C-D区域
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// 电芯正极极柱高度
        /// </summary>
        public decimal HeightZ { get; set; }

        /// <summary>
        /// 电芯负极极柱高度
        /// </summary>
        public decimal HeightF { get; set; }

        /// <summary>
        /// 电芯正极肩高
        /// </summary>
        public decimal ShoulderheightZ { get; set; }

        /// <summary>
        /// 电芯负极肩高
        /// </summary>
        public decimal ShoulderheightF { get; set; }

        /// <summary>
        /// 电芯厚度（包膜）
        /// </summary>
        public decimal Thickness { get; set; }
    }

    public record InteSFCBoxImportDto : BaseExcelDto
    {
        /// <summary>
        /// 档位
        /// </summary>
        [EpplusTableColumn(Header = "档位", Order = 1)]
        public string Grade { get; set; }

        /// <summary>
        /// 电芯条码
        /// </summary>
        [EpplusTableColumn(Header = "电芯条码", Order = 2)]
        public string SFC { get; set; }

        /// <summary>
        /// 箱码
        /// </summary>
        [EpplusTableColumn(Header = "箱码", Order = 3)]
        public string BoxCode { get; set; }

        /// <summary>
        /// 二次注液注液后重量
        /// </summary>
        [EpplusTableColumn(Header = "二次注液注液后重量", Order = 4)]
        public decimal Weight { get; set; }

        /// <summary>
        /// DC容量
        /// </summary>
        [EpplusTableColumn(Header = "DC容量", Order = 5)]
        public decimal DC { get; set; }

        /// <summary>
        /// DC容量数采时间
        /// </summary>
        [EpplusTableColumn(Header = "DC容量数采时间", Order = 6)]
        public string DCDate { get; set; }

        /// <summary>
        /// OCVB
        /// </summary>
        [EpplusTableColumn(Header = "OCVB", Order = 7)]
        public decimal OCVB { get; set; }

        /// <summary>
        /// OCVB数采时间
        /// </summary>
        [EpplusTableColumn(Header = "OCVB数采时间", Order = 8)]
        public string OCVBDate { get; set; }

        /// <summary>
        /// IMPB
        /// </summary>
        [EpplusTableColumn(Header = "IMPB", Order = 9)]
        public decimal IMPB { get; set; }

        /// <summary>
        /// 自放电率X
        /// </summary>
        [EpplusTableColumn(Header = "自放电率X", Order = 10)]
        public decimal SelfDischargeRate { get; set; }

        /// <summary>
        /// 电芯宽度（包膜）C-D区域
        /// </summary>
        [EpplusTableColumn(Header = "电芯宽度（包膜）C-D区域", Order = 11)]
        public decimal Width { get; set; }

        /// <summary>
        /// 电芯正极极柱高度
        /// </summary>
        [EpplusTableColumn(Header = "电芯正极极柱高度", Order = 12)]
        public decimal HeightZ { get; set; }

        /// <summary>
        /// 电芯负极极柱高度
        /// </summary>
        [EpplusTableColumn(Header = "电芯负极极柱高度", Order = 13)]
        public decimal HeightF { get; set; }

        /// <summary>
        /// 电芯正极肩高
        /// </summary>
        [EpplusTableColumn(Header = "电芯正极肩高", Order = 14)]
        public decimal ShoulderHeightZ { get; set; }

        /// <summary>
        /// 电芯负极肩高
        /// </summary>
        [EpplusTableColumn(Header = "电芯负极肩高", Order = 15)]
        public decimal ShoulderHeightF { get; set; }

        /// <summary>
        /// 电芯厚度（包膜）
        /// </summary>
        [EpplusTableColumn(Header = "电芯厚度（包膜）", Order = 16)]
        public decimal Thickness { get; set; }
    }


    public class UploadSFCBoxDto
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    public class InteSFCBoxQueryDto : PagerInfo
    {
        /// <summary>
        /// 电芯码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 箱码
        /// </summary>
        public string? BoxCode { get; set; }

        /// <summary>
        /// 批次码
        /// </summary>
        public string? BatchNo { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SFCBoxEnum? Status { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }
    }

    public record InteSFCBoxRView : BaseEntityDto
    {
        //public long? Id { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string? BatchNo { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        //public string? Grade { get; set; }
 
        public DateTime CreatedOn { get; set; }

    }

    /// <summary>
    /// 箱码验证请求
    /// </summary>
    public class InteSFCBoxValidateQuery
    {
        /// <summary>
        /// 箱码
        /// </summary>
        public string BoxCode { get; set; }
        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderCode { get; set; }
    }

    /// <summary>
    /// 箱码验证返回
    /// </summary>
   public class InteSFCBoxValidateResponse
    {
        public int State { get; set;}

        public string Msg {  get; set; }
    }


}
