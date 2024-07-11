﻿namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IOP010Service
    {
        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        Task ExecuteAsync(int limitCount = 1000);

    }
}