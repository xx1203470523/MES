using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 在制品拆解更新实体类
    /// </summary>
    public class DisassemblyCommand
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 流转类型
        /// </summary>
        public SfcCirculationTypeEnum CirculationType { get; set; }

        public TrueOrFalseEnum IsDisassemble {get;set;}

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }

    /// <summary>
    /// 条码关系表拆解更新实体类
    /// </summary>
    public class DManuBarCodeRelationCommand
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 关系类型
        /// </summary>
        public ManuBarCodeRelationTypeEnum RelationType { get; set; }

        public TrueOrFalseEnum IsDisassemble { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }

    /// <summary>
    /// 条码关系表拆解更新实体类
    /// </summary>
    public class DisassemBarCodeRelationblyCommand
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 流转类型
        /// </summary>
        public SfcCirculationTypeEnum CirculationType { get; set; }

        public TrueOrFalseEnum IsDisassemble { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }

}
