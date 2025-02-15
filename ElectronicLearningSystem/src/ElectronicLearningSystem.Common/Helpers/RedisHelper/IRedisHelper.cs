namespace ElectronicLearningSystem.Common.Helpers.RedisHelper
{
    public interface IRedisHelper
    {
        Task RecoveryPasswordAsync(string token, Guid id, TimeSpan expiration);

        Task<Guid?> GetUserIdByRecoveryPasswordTokenAsync(Guid id);
    }
}
