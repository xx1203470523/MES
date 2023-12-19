using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility.TemporaryStorage
{
    public class ManuSfcProduceTemporaryStorage : ITemporaryStorage<ManuSfcProduceEntity>
    {
        private readonly List<ManuSfcProduceEntity> _temporaryStorageList = new List<ManuSfcProduceEntity> { };

        public ManuSfcProduceTemporaryStorage()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ManuSfcProduceEntity> GetValue()
        {
            return _temporaryStorageList;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="Tparam"></typeparam>
       /// <param name="func"></param>
       /// <param name="parameter"></param>
       /// <param name="predicate"></param>
       /// <param name="expect"></param>
       /// <returns></returns>
        public IEnumerable<ManuSfcProduceEntity>? GetValue<Tparam>(Func<Tparam, IEnumerable<ManuSfcProduceEntity>> func, Tparam parameter,
            Func<ManuSfcProduceEntity, bool>? predicate = null, int? expect = 0)
        { 
            List<ManuSfcProduceEntity> list = new();
            if (predicate == null)
            {
                if (func != null)
                {
                    list = func(parameter).ToList();
                    if (list != null && list.Any())
                    {
                        foreach (var item in list)
                        {
                            var temporaryEntity = _temporaryStorageList.FirstOrDefault(x => x.Id == item.Id);
                            if (temporaryEntity == null)
                            {
                                _temporaryStorageList.Add(item);
                            }
                            else
                            {
                                var index = list.FindIndex(x => x.Id == item.Id);
                                list[index] = temporaryEntity;
                            }
                        }
                    }
                }
            }
            else
            {
                list = _temporaryStorageList.Where(predicate).ToList(); 
                var count =  list.Select(x=>x.Id).Distinct().Count();
                if (expect == 0 || count != expect)
                {
                    if (func != null)
                    {
                        list = func(parameter).ToList();
                        if (list != null && list.Any())
                        {
                            foreach (var item in list)
                            {
                                var temporaryEntity = _temporaryStorageList.FirstOrDefault(x => x.Id == item.Id);
                                if (temporaryEntity == null)
                                {
                                    _temporaryStorageList.Add(item);
                                }
                                else
                                {
                                    var index = list.FindIndex(x => x.Id == item.Id);
                                    list[index] = temporaryEntity;
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}
