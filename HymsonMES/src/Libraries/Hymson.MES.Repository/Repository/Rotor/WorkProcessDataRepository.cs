using Hymson.MES.BackgroundServices.CoreServices.Model.Rotor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.CoreServices.Repository.Rotor
{
    /// <summary>
    /// 参数仓储
    /// </summary>
    public class WorkProcessDataRepository : BaseRepository<WorkProcessDataEntity>, IWorkProcessDataRepository
    {
        /// <summary>
        /// 获取过站数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<WorkProcessDataEntity>> GetList(string sql)
        {
            var result = await SqlQueryToListAsync(sql);

            return result;
        }
    }
}
