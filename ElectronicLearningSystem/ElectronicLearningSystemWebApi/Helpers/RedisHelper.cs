﻿using StackExchange.Redis;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class RedisHelper(IConnectionMultiplexer redisConnection)
    {
        protected readonly IDatabase _database = redisConnection.GetDatabase();

        public async Task RecoveryPasswordAsync(string token, string login, TimeSpan expiration)
        {
            await _database.StringSetAsync($"password_reset:{token}", login, expiration);
        }
    }
}
