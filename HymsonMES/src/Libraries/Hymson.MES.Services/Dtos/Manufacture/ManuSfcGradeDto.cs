/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除）    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Process;

namespace Hymson.MES.Services.Dtos.Manufacture 
{
    public record ManuSfcGradeViewDto : BaseEntityDto
    {
        public long Id { get; set; }

        /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 最终挡位值
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 挡位组合
        /// </summary>
        public string GeadeGroup { get; set; }

        public IEnumerable<ManuSfcGradeDetailViewDto> manuSfcGradeDetails { get; set; }

    }

    /// <summary>
    /// 条码档位明细
    /// </summary>
    public record ManuSfcGradeDetailViewDto : BaseEntityDto
    {
        /// <summary>
        /// 待选工序Id
        /// </summary>
        public long ProduceId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProduceCode { get; set; }

        /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 档次
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 标准参数Id
        /// </summary>
        public long ParamId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParamUnit { get; set; }

        /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 包含最小值类型;1< 2.≤
        /// </summary>
        public ContainingTypeEnum? MinContainingType { get; set; }

        /// <summary>
        /// 包含最大值类型;1< 2.≥
        /// </summary>
        public ContainingTypeEnum? MaxContainingType { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

    }
}