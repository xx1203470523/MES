using Hymson.Infrastructure;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Hymson.Utils;
using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility
{
    /// <summary>
    /// 
    /// </summary>
    public class JobContextProxy : IDisposable, IJobContextProxy
    {
        /// <summary>
        /// 
        /// </summary>
        protected ConcurrentDictionary<uint, object> dictionary = new();

        private const string IncompleteKey = "incomplete";
        /// <summary>
        /// 
        /// </summary>
        private static SemaphoreSlim[]? _semaphores;

        /// <summary>
        /// 构造函数
        /// </summary>
        public JobContextProxy()
        {
            int num = Math.Max(Environment.ProcessorCount * 8, 32);
            _semaphores = new SemaphoreSlim[num];
            for (int i = 0; i < _semaphores.Length; i++)
            {
                _semaphores[i] = new SemaphoreSlim(1);
            }
        }

        /// <summary>
        /// 获取字典Key
        /// </summary>
        /// <returns></returns>
        public ICollection<uint> GetKeys()
        {
            return dictionary.Keys;
        }

        /// <summary>
        /// 设置数据库缓存
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TResult?> SetDataBaseValueAsync<T, TResult>(Func<T, Task<TResult>> func, T parameters)
        {
            var obj = await GetValueAsync<T, TResult>(func, parameters);
            if (obj == null) return default;

            foreach (var property in obj.GetType().GetProperties())
            {
                var cacheKey = (uint)$"{property.PropertyType}".GetHashCode();
                var value = property.GetValue(obj);
                if (value == null) continue;
                //集合
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && value is not string)
                {
                    var iEnumerableType = property.PropertyType.GetGenericArguments().FirstOrDefault();
                    if (iEnumerableType == null) continue;
                    if (iEnumerableType.BaseType == typeof(BaseEntity))
                    {
                        var cacheValue = Get(cacheKey);
                        if (cacheValue == null)
                        {
                            Set(cacheKey, value);
                        }
                        else
                        {
                            Type listTypeDefinition = typeof(List<>);
                            Type listType = listTypeDefinition.MakeGenericType(iEnumerableType.UnderlyingSystemType);
                            dynamic list = Activator.CreateInstance(listType);
                            if (list != null)
                            {
                                var newCacheValue = cacheValue as IEnumerable<BaseEntity> ?? throw new Exception();

                                foreach (object valueItem in (IEnumerable<object>)value)
                                {
                                    if (valueItem != null)
                                    {
                                        var newValueItem = valueItem as BaseEntity;
                                        var index = ((IEnumerable<BaseEntity>)newCacheValue).ToList().FindLastIndex(x => x.Id == newValueItem?.Id);
                                        if (index > -1)
                                        {
                                            var type = cacheValue.GetType();
                                            if (type != null)
                                            {
                                                // 获取集合类型的 get_Item 方法
                                                MethodInfo getItemMethod = type.GetMethod("get_Item");
                                                object item = getItemMethod?.Invoke(cacheValue, new object[] { index });

                                                var addMethod = list.GetType().GetMethod("Add");
                                                addMethod?.Invoke(list, new[] { item });
                                            }
                                        }
                                        else
                                        {
                                            var addMethod = list.GetType().GetMethod("Add");
                                            addMethod?.Invoke(list, new[] { valueItem });
                                        }
                                    }
                                }
                                Set(cacheKey, list);
                            }
                        }
                    }
                    else
                    {
                        var jobProxyAttribute = iEnumerableType.GetCustomAttribute<JobProxyAttribute>();
                        if (jobProxyAttribute == null) continue;
                        var cacheValue = Get(cacheKey);
                        if (cacheValue != null)
                        {
                            foreach (var valueItem in (IEnumerable)value)
                            {
                                if (!Merge(valueItem, cacheValue))
                                {
                                    var incompleteKey = (uint)$"{IncompleteKey}{property.PropertyType}".GetHashCode();
                                    IncompleteMerge(incompleteKey, valueItem);
                                }
                            }
                            Set(cacheKey, cacheValue);
                        }
                        else
                        {
                            foreach (var item in (IEnumerable)value)
                            {
                                var incompleteKey = (uint)$"{IncompleteKey}{property.PropertyType}".GetHashCode();
                                IncompleteMerge(incompleteKey, item);
                            }
                        }
                    }
                }
                else
                {
                    var enumerableType = typeof(IEnumerable<>).MakeGenericType(property.PropertyType);
                    cacheKey = (uint)$"{enumerableType}".GetHashCode();
                    if (property.PropertyType.BaseType == typeof(BaseEntity))
                    {
                        var cacheValue = Get(cacheKey);
                        if (cacheValue != null)
                        {
                            if (cacheValue != null)
                            {
                                var newCacheValue = cacheValue as IEnumerable<BaseEntity>;

                                if (newCacheValue != null)
                                {
                                    var newCacheValuelist = newCacheValue.ToList();
                                    var index = newCacheValuelist.FindIndex(x => x.Id == ((BaseEntity)value).Id);
                                    if (index > -1)
                                    {
                                        newCacheValuelist[index] = (BaseEntity)value;
                                    }
                                    else
                                    {
                                        newCacheValuelist.Add((BaseEntity)value);
                                    }
                                    Set(cacheKey, newCacheValuelist);
                                }
                            }
                        }
                        else
                        {
                            var newCacheValue = new List<BaseEntity>();
                            newCacheValue.Add((BaseEntity)value);
                            Set(cacheKey, newCacheValue);
                        }
                    }
                    else
                    {
                        var jobProxyAttribute = property.PropertyType.GetCustomAttribute<JobProxyAttribute>();
                        if (jobProxyAttribute == null) continue;
                        cacheKey = (uint)$"{jobProxyAttribute.TableEntity}".GetHashCode();
                        var cacheValue = Get(cacheKey);
                        if (cacheValue != null)
                        {
                            if (Merge(value, cacheValue))
                            {
                                Set(cacheKey, cacheValue);
                            }
                            else
                            {
                                var incompleteKey = (uint)$"{IncompleteKey}{enumerableType}".GetHashCode();
                                IncompleteMerge(incompleteKey, value);
                            }
                        }
                        else
                        {
                            var incompleteKey = (uint)$"{IncompleteKey}{enumerableType}".GetHashCode();
                            IncompleteMerge(incompleteKey, value);
                        }
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// 处理额外数据
        /// </summary>
        /// <param name="incompleteKey"></param>
        /// <param name="incompleteValue"></param>
        public void IncompleteMerge(uint incompleteKey, object value)
        {
            var incompleteCache = Get(incompleteKey);
            if (incompleteCache == null)
            {
                var incompleteValueList = new List<object> { value };
                Set(incompleteKey, incompleteValueList);
            }
            else
            {
                var incompletelist = ((IList<object>)incompleteCache) ?? throw new Exception();
                if (Merge(value, incompletelist))
                {
                    Set(incompleteKey, incompletelist);
                }
                else
                {
                    incompletelist.Add(value);
                    Set(incompleteKey, incompletelist);
                }
            }
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="source">合并原</param>
        /// <param name="target">目标</param>
        /// <returns></returns>
        private static bool Merge(object? source, object? target)
        {
            if (target == null || source == null) return false;
            var dicConditionField = new Dictionary<string, object?>();
            var dicField = new Dictionary<string, object?>();
            foreach (var itemProperty in source.GetType().GetProperties())
            {
                var itemValue = itemProperty.GetValue(source);
                var fieldName = itemProperty.Name;
                //字段名
                var field = itemProperty.GetCustomAttribute<FieldAttribute>();
                if (field != null)
                {
                    fieldName = field.Name;
                }

                //查询
                var primaryKey = itemProperty.GetCustomAttribute<ConditionAttribute>();
                if (primaryKey != null)
                {
                    dicConditionField.Add(fieldName, itemValue);
                }

                //更新字段   TODO 暂不支持集合更新
                var ignore = itemProperty.GetCustomAttribute<IgnoreAttribute>();
                if (ignore == null || !ignore.IsIgnore)
                {
                    dicField.Add(fieldName, itemValue);
                }
            }
            var isUpdated = false;
            foreach (var targetItem in (IEnumerable)target)
            {
                var isUpdate = false;
                var cacheItemType = targetItem.GetType();
                foreach (var field in dicConditionField)
                {
                    if (field.Value == null) continue;
                    var itemPrimaryKeyValue = cacheItemType.GetProperties().FirstOrDefault(x => x.Name == field.Key);
                    if (itemPrimaryKeyValue != null)
                    {
                        var itemValue = itemPrimaryKeyValue.GetValue(targetItem);
                        if (typeof(IEnumerable).IsAssignableFrom(field.Value.GetType()) && field.Value is not string)
                        {
                            foreach (var fieldValue in (IEnumerable)field.Value)
                            {
                                if (itemValue?.ToString() == fieldValue.ToString())
                                {
                                    isUpdate = true;
                                    break;
                                }
                                isUpdate = false;
                            }
                        }
                        else
                        {
                            if (itemValue?.ToString() == field.Value?.ToString())
                            {
                                isUpdate = true;
                                break;
                            }
                        }
                    }

                    if (!isUpdate) break;
                }
                if (isUpdate)
                {
                    foreach (var field in dicField)
                    {
                        var cacheItemProperty = cacheItemType.GetProperty(field.Key);
                        if (cacheItemProperty != null)
                        {
                            cacheItemProperty.SetValue(targetItem, field.Value);
                        }
                    }
                }
                isUpdated = isUpdated && isUpdate;
            }

            return isUpdated;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incompleteKey"></param>
        /// <param name="target"></param>
        private void IncompleteDatabaseMerge<TResult>(uint incompleteKey, IEnumerable<TResult>? target) where TResult : BaseEntity
        {
            var incompleteCache = Get(incompleteKey);
            if (incompleteCache == null || target == null) return;
            var incompleteValueList = new List<object>();
            foreach (var item in (IEnumerable)incompleteCache)
            {
                if (!Merge(incompleteCache, target))
                {
                    incompleteValueList.Add(item);
                }
            }
            Set(incompleteKey, incompleteValueList);
        }

        /// <summary>
        /// 获取作业中入库的数据缓存
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>?> GetDataBaseValueAsync<T, TResult>(Func<T, Task<IEnumerable<TResult>>> func, T parameters, int expectCount = 0) where TResult : BaseEntity
        {
            var cacheKey = (uint)$"{typeof(IEnumerable<TResult>)}".GetHashCode();

            if (Has(cacheKey))
            {
                var cacheObj = Get(cacheKey);
                if (cacheObj == null) return default;
                var cacheResult = (IEnumerable<TResult>)cacheObj;

                if (cacheResult == null || cacheResult.Any() == false)
                {
                    try
                    {
                        cacheResult = await GetValueAsync(func, parameters);
                        cacheResult ??= (IEnumerable<TResult>)cacheObj;
                    }
                    finally { }
                }
                if (expectCount != 0 && cacheResult.Count() < expectCount)
                {
                    var obj = await GetValueAsync(func, parameters);
                    if (obj != null)
                    {
                        IncompleteDatabaseMerge(cacheKey, obj);

                        _ = cacheResult.Concat(obj.Where(x => !cacheResult.Any(o => o.Id == x.Id)));
                        Set(cacheKey, cacheResult);
                    }
                }
                return cacheResult;
            }

            try
            {
                var obj = await GetValueAsync(func, parameters);
                if (obj == null) return default;
                Set(cacheKey, obj);
                return obj;
            }
            finally { }
        }





        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public TResult? GetValue<T, TResult>(Func<T, TResult> func, T parameter)
        {
            var paramString = "";
            if (parameter != null && parameter.IsNotEmpty()) paramString = parameter.ToSerialize();

            var cacheKey = (uint)$"{func.Method.DeclaringType?.FullName}.{func.Method.Name}{paramString}".GetHashCode();

            if (Has(cacheKey))
            {
                var cacheObj = Get(cacheKey);
                if (cacheObj == null) return default;

                return (TResult)cacheObj;
            }

            var tResult = func(parameter);
            if (tResult == null) return default;

            Set(cacheKey, tResult);
            return tResult;
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<TResult?> GetValueAsync<T, TResult>(Func<T, Task<TResult>> func, T parameter)
        {
            var paramString = "";
            if (parameter != null && parameter.IsNotEmpty()) paramString = parameter.ToSerialize();

            var cacheKey = (uint)$"{func.Method.DeclaringType?.FullName}.{func.Method.Name}{paramString}".GetHashCode();
            if (Has(cacheKey))
            {
                var cacheObj = Get(cacheKey);
                if (cacheObj == null) return default;

                return (TResult)cacheObj;
            }

            var tResult = await func(parameter);
            if (tResult == null) return default;

            Set(cacheKey, tResult);
            return tResult;
        }

        /// <summary>
        /// 存放
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Set(uint key, object value)
        {

            return dictionary.AddOrUpdate(key, value, (k, v) => value);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object? Get(uint key)
        {
            return dictionary.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool Has(uint key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool Remove(uint key)
        {
            return dictionary.TryRemove(key, out _);
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            dictionary.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
