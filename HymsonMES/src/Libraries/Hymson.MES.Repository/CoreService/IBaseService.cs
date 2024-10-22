﻿using Hymson.MES.BackgroundServices.CoreServices.Repository;

namespace Hymson.MES.BackgroundServices.CoreServices
{
    /// <summary>
    /// 基础服务定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseService<T> : IBaseRepository<T> where T : class, new()
    {
    }
}