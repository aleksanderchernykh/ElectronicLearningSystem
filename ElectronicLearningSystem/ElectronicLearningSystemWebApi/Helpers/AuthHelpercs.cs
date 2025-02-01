using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using ElectronicLearningSystemWebApi.Repositories.User;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class AuthHelper(IUserRepository userRepository,
        UserHelper userHelper,
        JwtTokenHelper tokenHelper)
    {
        private readonly IUserRepository _userRepository = userRepository 
            ?? throw new ArgumentNullException(nameof(_userRepository));

        private readonly UserHelper _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(_userHelper));

        private readonly JwtTokenHelper _tokenHelper = tokenHelper
            ?? throw new ArgumentNullException(nameof(_tokenHelper));

        public async Task<AccessTokenResponse> LoginAsync(UserLoginDTO userLoginRequest)
        {
            if(userLoginRequest is null)
                throw new UnauthorizedAccessException("Invalid request data");

            var user = await _userRepository.GetUserByLoginAsync(userLoginRequest.Login);
            if (user == null || !_userHelper.VerificationPassword(user, userLoginRequest.Password))
            {
                throw new UnauthorizedAccessException("The user entered the wrong username or password");
            }

            if (user.IsLocked)
            {
                throw new InvalidOperationException("The user's account has been deactivated");
            }

            var (AccessToken, RefreshToken) = await _tokenHelper.GenerateTokenForUser(user);

            return new AccessTokenResponse 
            { 
                AccessToken = AccessToken, 
                RefreshToken = RefreshToken 
            };
        }

        public async Task LogoutAsync(LogoutDTO logoutDTO)
        {

            var user = await _userRepository.GetUserByLoginAsync(logoutDTO.Login) 
                ?? throw new UnauthorizedAccessException("The user entered the wrong username");

            await _userHelper.LogoutUserAsync(user);
        }

        public async Task<AccessTokenResponse> RefreshTokenAsync(RefreshTokenDTO refreshTokenRequest)
        {
            var principal = _tokenHelper.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken) 
                ?? throw new UnauthorizedAccessException("Invalid access token");

            var user = await _userRepository.GetUserByLoginAsync(principal?.Identity?.Name) 
                ?? throw new UnauthorizedAccessException("The user was not found using the transferred token");

            if (user.RefreshToken != refreshTokenRequest.RefreshToken)
            {
                throw new UnauthorizedAccessException("Invalid user's refresh token");
            }

            var (AccessToken, RefreshToken) = await _tokenHelper.GenerateTokenForUser(user);

            return new AccessTokenResponse
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };
        }

        internal async Task RecoveryPasswordAsync(RecoveryPasswordDTO recoveryPasswordDTO)
        {
            /*if (recoveryPasswordDTO.Login == null)
            {


                return BadRequest(new
                {
                    ErrorCode = "INVALID_REQUEST",
                    Message = "Invalid request data"
                });
            }

            var user = await _userRepository.GetUserByLoginAsync(login.Login);
            if (user is null)
            {
                return StatusCode(404, new
                {
                    ErrorCode = "NOT_FOUND",
                    Message = "User was not found"
                });
            }*/
        }
    }
}
