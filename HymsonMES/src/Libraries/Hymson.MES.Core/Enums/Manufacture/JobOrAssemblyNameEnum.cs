using Hymson.MES.Core.Attribute.Manufacture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 程序集或者作业名称
    /// </summary>
    public enum JobOrAssemblyNameEnum : short
    {
        /// <summary>
        /// 批次下达
        /// </summary>
        [Description("批次下达")]
        BatchCreate = 1,

        /// <summary>
        /// 条码接收
        /// </summary>
        [Description("条码接收")]
        Receive = 2,

        /// <summary>
        /// 进站作业
        /// </summary>
        [Description("进站作业")]
        InStock = 3,

        /// <summary>
        /// 进站作业
        /// </summary>
        [Description("出站作业")]
        OutStock = 4,

        /// <summary>
        /// 质量锁定
        /// </summary>
        [Description("质量锁定")]
        QualityLocking = 5,

        /// <summary>
        /// 不良录入
        /// </summary>
        [Description("不良录入")]
        BadEntry = 6,

        /// <summary>
        /// 条码报废
        /// </summary>
        [Description("条码报废")]
        Discard = 7,

        /// <summary>
        /// 停止作业
        /// </summary>
        [Description("停止作业")]
        Stop = 8,

        /// <summary>
        /// 不良复判
        /// </summary>
        [Description("不良复判")]
        BadRejudgment = 11,


        /// <summary>
        /// 在制品维修
        /// </summary>
        [Description("在制品维修")]
        Repair = 12,

        /// <summary>
        /// 步骤控制
        /// </summary>
        [Description("步骤控制")]
        StepControl = 13,
        /// <summary>
        /// 生产更改
        /// </summary>
        [Description("生产更改")]
        ManuUpdate = 14,

        /// <summary>
        /// 组件配置
        /// </summary>
        [Description("组件配置")]
        ComponentConfiguration = 15,

        /// <summary>
        /// 条码合并
        /// </summary>
        [Description("条码合并")]
        SfcMerge = 16,

        /// <summary>
        /// 条码数量调整
        /// </summary>
        [Description("条码数量调整")]
        SfcQtyAdjust = 17,
        /// <summary>
        /// 条码拆分
        /// </summary>
        [Description("条码拆分")]
        Split = 18,

        /// <summary>
        /// 降级录入
        /// </summary>
        [Description("降级录入")]
        EnterDowngrading = 19,

        /// <summary>
        /// 降级移除
        /// </summary>
        [Description("降级移除")]
        RemoveDowngrading = 20,


        /// <summary>
        /// Marking
        /// </summary>
        [Description("Marking录入")]
        Marking = 21,

        /// <summary>
        /// 关闭Marking
        /// </summary>
        [Description("关闭Marking")]
        CloseMarking = 22,

        /// <summary>
        /// 参数采集
        /// </summary>
        [Description("参数采集")]
        ParameterCollect = 23,

        /// <summary>
        /// 装箱
        /// </summary>
        [Description("装箱")]
        Packing = 24
    }
}
