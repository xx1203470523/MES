using System.ComponentModel;
/// <summary>
/// 发布版本
/// </summary>
public enum EnvironmentTypeEnum : short
{
    /// <summary>
    /// 开发环境
    /// </summary>
    [Description("开发环境")]
    dev = 1,

    /// <summary>
    /// 测试环境
    /// </summary>
    [Description("测试环境")]
    test = 2,

    /// <summary>
    /// 生产环境
    /// </summary>
    [Description("生产环境")]
    prod = 3,
}