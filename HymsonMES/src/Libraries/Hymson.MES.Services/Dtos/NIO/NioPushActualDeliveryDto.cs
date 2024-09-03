using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.CoreServices.Helper;

namespace Hymson.MES.Services.Dtos.NIO
{
    /// <summary>
    /// 物料发货信息表新增/更新Dto
    /// </summary>
    public record NioPushActualDeliverySaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public long NioPushId { get; set; }

       /// <summary>
        /// 合作业务（1:电池，2:电驱）
        /// </summary>
        public int PartnerBusiness { get; set; }

       /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

       /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

       /// <summary>
        /// 实际发货数量
        /// </summary>
        public decimal ShippedQty { get; set; }

       /// <summary>
        /// 实际发货时间
        /// </summary>
        public long? ActualDeliveryTime { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public string Date { get; set; }
    }

    /// <summary>
    /// 物料发货信息表Dto
    /// </summary>
    public record NioPushActualDeliveryDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public long NioPushId { get; set; }

       /// <summary>
        /// 合作业务（1:电池，2:电驱）
        /// </summary>
        public int PartnerBusiness { get; set; }

       /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

       /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

       /// <summary>
        /// 实际发货数量
        /// </summary>
        public decimal ShippedQty { get; set; }

       /// <summary>
        /// 实际发货时间
        /// </summary>
        public long? ActualDeliveryTime { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime Date 
        {
            get 
            {
                return NioHelper.UnixTimestampMillisToDateTime((long)ActualDeliveryTime);
            }
        }

       /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum Status { get; set; }
    }

    /// <summary>
    /// 物料发货信息表分页Dto
    /// </summary>
    public class NioPushActualDeliveryPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ?MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ?MaterialName { get; set; }

        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum ?Status { get; set; }
    }

}
