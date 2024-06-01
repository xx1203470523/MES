using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.SystemServices.Dtos
{

    /// <summary>
    /// 包装
    /// </summary>
    public record PackingReqDto : BaseEntityDto
    {
        /// <summary>
        /// 包装编码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 包装类型1:载具 ，2：包装
        /// </summary>
        //public PackagingWarehouTypeEnum Type { get; set; }
    }

    /// <summary>
    /// 包装
    /// </summary>
    public record PackingDto : BaseEntityDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 生产订单分录号
        /// </summary>
        public string FentryId { get; set; }

        /// <summary>
        /// 包装条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 电芯条码
        /// </summary>
        public string LadeBarCode { get; set; }

        /// <summary>
        /// 外箱条码
        /// </summary>
        public string OutBarcode { get; set; } = "";

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime ProduceDate { get; set; }

        /// <summary>
        /// 电芯数量
        /// </summary>
        public decimal Qty { get; set; }

       // public List<PackingDto> Childs { get; set; }
    }

    /// <summary>
    /// 包装
    /// </summary>
    public record PackingViewDto:BaseEntityDto
    {
        /// <summary>
        /// 容器条码id
        /// </summary>
        public long ContainerBarCodeId { get; set; }

        /// <summary>
        /// 装载条码
        /// </summary>
        public string LadeBarCode { get; set; }

        public long BarCodeId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
