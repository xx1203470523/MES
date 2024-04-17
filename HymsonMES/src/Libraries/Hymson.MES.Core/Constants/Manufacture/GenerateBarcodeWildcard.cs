using Hymson.MES.Core.Attribute;
using Hymson.MES.Core.Enums.Integrated;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Hymson.MES.Core.Constants.Manufacture
{
    /// <summary>
    /// 生成条码通配符
    /// </summary>
    public static class GenerateBarcodeWildcard
    {
        /// <summary>
        /// 流水
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("序号", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string Activity = "%ACTIVITY%";

        /// <summary>
        /// 日期
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("日期", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string Yymmdd = "%YYMMDD%";

        /// <summary>
        /// 
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("多个", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More })]
        public const string MultipleVariable = "%MULTIPLE_VARIABLE%";

        /// <summary>
        /// 年月日通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("年月日通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string YMDWildcard = "%YMD_WILDCARD%";

        /// <summary>
        /// 线别通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("线别通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string LINETYPE = "%LINE_TYPE%";

        /// <summary>
        /// 电池规格通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("电池规格通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string BatterySpecifications = "%BATTERYSPECIFICATIONS%";

        /// <summary>
        /// 当前时间的年份通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("当前时间的年份通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string SingleYearDirect = "%SINGLEYEARDIRECT%";

        /// <summary>
        /// 当前时间的月份通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("当前时间的月份通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string SingleMonthDirect = "%SINGLEMONTHDIRECT%";

        /// <summary>
        /// 当前时间的天通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("当前时间的天通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string SingleDayDirect = "%SINGLEDAYDIRECT%";


        /// <summary>
        /// 当前时间的年份映射通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("当前时间的年份映射通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string SingleYearMapping = "%SINGLEYEARMAPPING%";

        /// <summary>
        /// 当前时间的月份映射通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("当前时间的月份映射通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string SingleMonthMapping = "%SINGLEMONTHMAPPING%";

        /// <summary>
        /// 当前时间的天映射通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("当前时间的天映射通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string SingleDayMapping = "%SINGLEDAYMAPPING%";

        /// <summary>
        /// 正极主料通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("正极主料通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string AnodeMain = "%ANODEMAIN%";


        /// <summary>
        /// 负极主料通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("负极主料通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string CathodeMain = "%CATHODEMAIN%";


        /// <summary>
        /// 隔膜通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("隔膜通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string Diaphragm = "%DIAPHRAGM%";


        /// <summary>
        /// 正极极片通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("正极极片通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string PositivePlate = "%POSITIVEPLATE%";

        /// <summary>
        /// 极片状态通配符
        /// </summary>
        [GenerateBarcodeWildcardDescriptionAttribute("极片状态通配符", new CodeRuleCodeTypeEnum[] { CodeRuleCodeTypeEnum.ProcessControlSeqCode, CodeRuleCodeTypeEnum.PackagingSeqCode, CodeRuleCodeTypeEnum.IQC, CodeRuleCodeTypeEnum.OQC, CodeRuleCodeTypeEnum.FQC }, new CodeRuleCodeModeEnum[] { CodeRuleCodeModeEnum.More, CodeRuleCodeModeEnum.One })]
        public const string ElectrodeState = "%ELECTRODESTATE%";
    }
}
