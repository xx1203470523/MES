using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 生成检验单Dto
    /// </summary>
    public record GenerateInspectionDto
    {
        /// <summary>
        /// 收货单
        /// </summary>
        public long ReceiptId { get; set; }

        /// <summary>
        /// 收货单
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 收货单明细ID集合
        /// </summary>
        public IEnumerable<long> Details { get; set; }

    }

    /// <summary>
    /// 检验参数Dto
    /// </summary>
    public record InspectionParameterDetailDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数Id proc_parameter 的id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数数据类型
        /// </summary>
        public DataTypeEnum ParameterDataType { get; set; }

        /// <summary>
        /// 检验器具
        /// </summary>
        public int Utensil { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal UpperLimit { get; set; }

        /// <summary>
        /// 规格中心
        /// </summary>
        public decimal Center { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal LowerLimit { get; set; }

        /// <summary>
        /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
        /// </summary>
        public IQCInspectionTypeEnum InspectionType { get; set; }

    }

}
