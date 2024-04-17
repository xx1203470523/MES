using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility.TemporaryStorage
{
    /// <summary>
    /// 存储器
    /// </summary>
    public interface ITemporaryStorage<T> where T : BaseEntity , new()
    {
        /// <summary>
        /// 获取存储器所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetValue();

        /// <summary>
        /// 获取存储中数据 通过筛选器过来数据，满足条件则更新存储器不满足执行方法，然后更新缓存 暂定以缓存为主
        /// </summary>
        /// <typeparam name="Tparam"></typeparam>
        /// <param name="func"></param>
        /// <param name="parameter"></param>
        /// <param name="predicate"></param>
        /// <param name="filterFunc"></param>
        /// <returns></returns>
        public IEnumerable<T>? GetValue<Tparam>(Func<Tparam, IEnumerable<T>> func, Tparam parameter,
             Func<T, bool>? predicate = null, int? expect = 0);
    }
}
