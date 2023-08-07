namespace Hymson.MES.EquipmentServicesTests.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// 随机获取List中的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="numberOfElements">获取前几个</param>
        /// <returns></returns>
        public static List<T> GetListRandomElements<T>(this List<T> list, int numberOfElements)
        {
            if (numberOfElements > list.Count)
            {
                throw new ArgumentException("numberOfElements 不能大于List集合长度");
            }
            Random random = new();
            // 使用洗牌算法对List进行随机排序
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            // 随机获取List中的前几个元素
            return list.Take(numberOfElements).ToList();
        }
    }
}
