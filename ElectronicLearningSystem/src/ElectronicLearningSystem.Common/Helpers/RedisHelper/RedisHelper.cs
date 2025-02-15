using StackExchange.Redis;

namespace ElectronicLearningSystem.Common.Helpers.RedisHelper
{
    public class RedisHelper(IConnectionMultiplexer redisConnection) : IRedisHelper
    {
        protected readonly IDatabase _database = redisConnection.GetDatabase();

        public async Task RecoveryPasswordAsync(string token, Guid id, TimeSpan expiration)
        {
            await _database.StringSetAsync($"password_reset:{token}", id.ToString(), expiration);
        }

        public async Task<Guid?> GetUserIdByRecoveryPasswordTokenAsync(Guid id)
        {
            RedisValue value = await _database.StringGetAsync($"password_reset:{id}");

            if (value.IsNullOrEmpty)
                return null;

            if (Guid.TryParse(value.ToString(), out Guid userId))
                return userId;

            return null;
        }
    }
}
