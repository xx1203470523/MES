namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public interface IBaseService
    {
        /// <summary>
        /// 获取基础配置（定子）
        /// </summary>
        /// <returns></returns>
        Task<BaseStatorBo> GetStatorBaseConfigAsync();

        /// <summary>
        /// 保存转换数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> SaveConvertDataAsync<T>(IEnumerable<BaseOPEntity> entities);

    }
}
