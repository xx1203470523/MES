using Hymson.MES.BackgroundServices.CoreServices.Model.Rotor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.CoreServices.Repository.Rotor
{
    /// <summary>
    /// 过站数据
    /// </summary>
    public interface IWorkProcessRepository
    {
        /// <summary>
        /// 获取过站数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<WorkProcessEntity>> GetList(string sql);
    }
}
