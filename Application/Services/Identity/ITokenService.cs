using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;

namespace Application.Services.Identity
{
    public interface ITokenService
	{
		Task<ResponseWrapper<TokenResponse>> GetTokenAsync(TokenRequest request);
		Task<ResponseWrapper<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest request);
	}
}
