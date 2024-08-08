using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.NioPushCollection
{
    /// <summary>
    /// 数据实体（NIO推送参数）   
    /// nio_push_collection
    /// @author Yxx
    /// @date 2024-08-05 04:09:48
    /// </summary>
    public class NioPushCollectionEntity : BaseEntity
    {
        /// <summary>
        /// NIO推送ID
        /// </summary>
        public long NioPushId { get; set; }

        /// <summary>
        /// 工厂唯一标识
        /// </summary>
        public string PlantId { get; set; }

        /// <summary>
        /// 车间唯一标识
        /// </summary>
        public string WorkshopId { get; set; }

        /// <summary>
        /// 产线唯一标识
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 工位唯一标识
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 对应控制项主数据中的字段
        /// </summary>
        public string VendorFieldCode { get; set; }

        /// <summary>
        /// 合作伙伴总成产品代码
        /// </summary>
        public string VendorProductNum { get; set; }

        /// <summary>
        /// 合作伙伴总成物料名称
        /// </summary>
        public string VendorProductName { get; set; }

        /// <summary>
        /// 合作伙伴总成序列号
        /// </summary>
        public string VendorProductSn { get; set; }

        /// <summary>
        /// 合作伙伴总成临时序列号
        /// </summary>
        public string VendorProductTempSn { get; set; }

        /// <summary>
        /// 值的作用类型
        /// </summary>
        public string ProcessType { get; set; }

        /// <summary>
        /// 采集的控制项的值（当 valueType = decimal 时）
        /// </summary>
        public decimal? DecimalValue { get; set; }

        /// <summary>
        /// 采集的控制项的值（当 valueType = string 时）
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// 采集的控制项的值（当 valueType = boolean 时）
        /// </summary>
        public bool? BooleanValue { get; set; }

        /// <summary>
        /// NIO 产品代码
        /// </summary>
        public string NioProductNum { get; set; }

        /// <summary>
        /// NIO 产品名称
        /// </summary>
        public string NioProductName { get; set; }

        /// <summary>
        /// 操作员账号
        /// </summary>
        public string OperatorAccount { get; set; }

        /// <summary>
        /// 投入时间，Unix 时间戳
        /// </summary>
        public long InputTime { get; set; }

        /// <summary>
        /// 产出时间，Unix 时间戳
        /// </summary>
        public long OutputTime { get; set; }

        /// <summary>
        /// 标准化工位通过状态
        /// </summary>
        public string StationStatus { get; set; }

        /// <summary>
        /// 合作伙伴自定义的工位的通过状态
        /// </summary>
        public string VendorStationStatus { get; set; }

        /// <summary>
        /// 合作伙伴自定义的本条数据的通过状态
        /// </summary>
        public string VendorValueStatus { get; set; }

        /// <summary>
        /// EDS
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 数据类型 1-转子 2-定子
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public TrueOrFalseEnum IsOk { get; set; } = TrueOrFalseEnum.Yes;
    }
}
