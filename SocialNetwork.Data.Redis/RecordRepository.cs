using System;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using StackExchange.Redis;

namespace SocialNetwork.Data.Redis
{
    public class RecordRepository
    {
        public static ConnectionMultiplexer _connection = ConnectionMultiplexer.Connect(Helper.CnnValRedis());
        public static void SetRecord<T>(string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null)
        {
            var jsonData = JsonSerializer.Serialize(data);
            _connection.GetDatabase().StringSet(recordId, jsonData, absoluteExpireTime);
        }

        public static T GetRecord<T>(string recordId)
        {
            RedisValue jsonData = _connection.GetDatabase().StringGet(recordId);
            if (jsonData == RedisValue.Null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
//