using Newtonsoft.Json;

namespace Hymson.MES.BackgroundServices.NIO.Dtos
{
    /// <summary>
    /// 基类
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// 是否已投产, true/false
        /// </summary>
        public bool Debug { get; set; } = true;

        /// <summary>
        /// 更改的时间, Unix 时间戳, 以秒为单位
        /// </summary>
        public long UpdateTime { get; set; }

    }

    /// <summary>
    /// 基类（请求）
    /// </summary>
    public class NIORequestDto<T>
    {
        /// <summary>
        /// 业务场景编码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        public IEnumerable<T> List { get; set; }
    }

    /// <summary>
    /// 基类（响应）
    /// </summary>
    public class NIOResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public NIOResponseBaseDto NexusOpenapi { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 基类（响应）
    /// </summary>
    public class NIOResponseBaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string ReqId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

    }

    /// <summary>
    /// NIO配置基础数据
    /// </summary>
    public class NIOConfigBaseDto
    {
        /// <summary>
        /// 1-转子 2-定子
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 工厂唯一标识
        /// </summary>
        public string PlantId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string PlantName { get; set; }

        /// <summary>
        /// 车间唯一标识
        /// </summary>
        public string WorkshopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 生产线唯一标识
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 生产线名称
        /// </summary>
        public string ProductionLineName { get; set; }

        /// <summary>
        /// 合合作伙伴产品代码
        /// </summary>
        public string VendorProductCode { get; set; }

        /// <summary>
        /// 合作伙伴产品名称
        /// </summary>
        public string VendorProductName { get; set; }

        /// <summary>
        /// NIO产品代码
        /// </summary>
        public string NioProductCode { get; set; }

        /// <summary>
        /// NIO产品名称
        /// </summary>
        public string NioProductName { get; set; }

        /// <summary>
        /// 产品一次良率
        /// </summary>
        public string PassRateTarget { get; set; }
    }
}
