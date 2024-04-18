using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 产出修改业务实体
    /// </summary>
    public class OutputModifyBo : JobBaseBo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        public IEnumerable<OutputModifySFCBo> SfcList { get; set; }
    }

    /// <summary>
    /// 产出条码信息
    /// </summary>
    public class OutputModifySFCBo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal QualifiedQty { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal UnQualifiedQty { get; set; }
    }
    public class SFCPartialScrapResponseBo
    {
        public List<ManuSfcScrapEntity>  manuSfcScrapEntities { get; set; }
        public List<ManuSFCPartialScrapByIdCommand> manuSFCPartialScrapByIdCommandList { get; set; }
        public List<ManuSfcStepEntity> manuSfcStepEntities { get; set; }

       
    }

    /// <summary>
    /// 
    /// </summary>
    public class OutputModifyResponseBo
    {
        /// <summary>
        /// 条码信息
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity> ManuSfcProduceEntities { get; set; } = new List<ManuSfcProduceEntity>();

        /// <summary>
        /// 步骤信息
        /// </summary>
        public IEnumerable<ManuSfcStepEntity> ManuSfcStepEntities { get; set; } =new  List<ManuSfcStepEntity>();

        /// <summary>
        /// 更新在制品信息
        /// </summary>
        public IEnumerable<UpdateSfcProcedureQtyByIdCommand>  UpdateSfcProcedureQtyByIdCommands=new List<UpdateSfcProcedureQtyByIdCommand>();

        /// <summary>
        /// 更新条码表信息
        /// </summary>
        public IEnumerable<UpdateManuSfcQtyByIdCommand> UpdateManuSfcQtyByIdCommands=new List<UpdateManuSfcQtyByIdCommand>();
    }
}
