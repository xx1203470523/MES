using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Entity
{
    /// <summary>
    /// 过站参数数据
    /// </summary>
    [Table("Work_ProcessData")]
    public class WorkProcessDataEntity
    {
        /// <summary>
        /// ID 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 工艺流程唯一标识
        /// </summary>
        public string ProcessUID { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        /// 工位编号
        /// </summary>
        public string WorkPosNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string NameCode { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        public float? Value { get; set; }

        /// <summary>
        /// 字符串值
        /// </summary>
        public string StrValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public float? MinValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public float? MaxValue { get; set; }

        /// <summary>
        /// 标准值
        /// </summary>
        public float? StandardValue { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public int? Result { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 字符串结果
        /// </summary>
        public string StrResult { get; set; }
    }
}
