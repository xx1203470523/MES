using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services
{
    /// <summary>
    /// 条码追溯服务接口
    /// </summary>
    public interface ITracingSourceSFCService
    {
        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<NodeSourceDto> SourceAsync(string sfc);

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<NodeSourceDto> DestinationAsync(string sfc);


        /// <summary>
        /// 查询此条码所有经过的工序相关信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcedureSourceDto>> GetProcedureSourcesAsync(string sfc);


        /// <summary>
        /// 查询此条码所有的作业日志
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<StepSourceDto>> GetStepSourcesAsync(string sfc);

        /// <summary>
        /// 查询此条码 产品参数 追溯DTO
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ProductParameterSourceDto>> GetProductParameterSourcesAsync(string sfc);


        /// <summary>
        /// 查询此条码原材料追溯信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<MaterialSourceDto>> GetMaterialSourcesAsync(string sfc);

        /// <summary>
        /// 查询此条码生产开始 结束中间设备参数
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<EquipmentParameterSourceDto>> GetEquipmentParameterSourcesAsync(string sfc);

    }
}