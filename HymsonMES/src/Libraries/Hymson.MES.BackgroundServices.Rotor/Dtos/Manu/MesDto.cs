using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Dtos.Manu
{
    /// <summary>
    /// MES数据基础实体
    /// </summary>
    public class MesDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool IsPassed { get; set; }
    }

    /// <summary>
    /// MES过站数据
    /// </summary>
    public class MesOutDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 进站时间
        /// </summary>
        public DateTime InDate { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool IsPassed { get; set; }

        /// <summary>
        /// 0-中间参数 1-进站 2-出站
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 上料条码列表
        /// </summary>
        public List<SfcUpMatDto> UpMatList {  get; set; } = new List<SfcUpMatDto>();

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<SfcParamDto> ParamList { get; set; } = new List<SfcParamDto>();

        /// <summary>
        /// NG列表
        /// </summary>
        public List<string> NgList { get; set; }

        /// <summary>
        /// 参数转NG信息
        /// </summary>
        public void ParamToNgList()
        {
            NgList = ParamList.Where(m => m.Result == false).Select(m => m.ParamName).ToList();
        }
    }

    /// <summary>
    /// 条码上的物料
    /// </summary>
    public class SfcUpMatDto
    {
        /// <summary>
        /// 物料类型
        /// 1-唯一条码 2-批次条码
        /// </summary>
        public int MatType { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 最终物料编码
        /// </summary>
        public string MatCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MatName { get; set; }

        /// <summary>
        /// 上的物料条码
        /// </summary>
        public string MatValue { get; set; }
        
        /// <summary>
        /// 物料批次条码
        /// </summary>
        public string MatBatchCode { get; set; }

        /// <summary>
        /// 物料数量
        /// </summary>
        public int? MatNum { get; set; }

        /// <summary>
        /// 主物料编码
        /// </summary>
        public string MainMatCode { get; set; }
    }

    /// <summary>
    /// 条码参数
    /// </summary>
    public class SfcParamDto
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamCode { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 值类型
        /// 1-数字 2-字符串
        /// </summary>
        public int ValueType { get; set; } = 1;

        /// <summary>
        /// 参数值
        /// 处理后的值
        /// </summary>
        public string ParamValue { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public decimal Value { get; set; } = 0;

        /// <summary>
        /// 字符串值
        /// </summary>
        public string StrValue { get; set; }

        /// <summary>
        /// 结果
        /// 1-OK 其他-NG
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }
    }

}
