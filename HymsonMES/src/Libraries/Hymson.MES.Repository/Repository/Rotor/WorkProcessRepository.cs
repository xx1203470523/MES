using Hymson.MES.BackgroundServices.CoreServices.Model.Rotor;
using Hymson.MES.Data.Repositories.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.CoreServices.Repository.Rotor
{
    /// <summary>
    /// 过站仓储
    /// </summary>
    public class WorkProcessRepository : BaseRepository<WorkProcessEntity>, IWorkProcessRepository
    {
        /// <summary>
        /// 获取过站信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<WorkProcessEntity>> GetList(string sql)
        {
            var result = await SqlQueryToListAsync(sql);

            return result;
        }
    }
}
