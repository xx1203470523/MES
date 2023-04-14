using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuValidatorDto
{
    /// <summary>
    /// 条码锁操作验证实体
    /// </summary>
    public class ManuSfcLockValidatorDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { set; get; }

       /// <summary>
       /// 操作类型
       /// </summary>
        public QualityLockEnum OperationType { set; get; }
    }
}
