﻿using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Repositories
{
    /// <summary>
    /// 装箱仓储
    /// </summary>
    public interface IPackListRepository
    {
        /// <summary>
        /// 获取过站数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<PackListDto>> GetList(string sql);
    }
}
