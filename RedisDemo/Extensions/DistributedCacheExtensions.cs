using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisDemo.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task setRecordAsync<T>(this IDistributedCache cache,
            string recordID,
            T data,
            TimeSpan? absoluteExpireTime=null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = unusedExpireTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordID, jsonData, options);
        }
        public static async Task<T> getRecordAsync<T>(this IDistributedCache cache,
            string recordID){
            var jsonData = await cache.GetStringAsync(recordID);
            if (jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }


    }
}
