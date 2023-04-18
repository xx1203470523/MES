namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManufactureJobService
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="extra"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string? extra);
    }
}
