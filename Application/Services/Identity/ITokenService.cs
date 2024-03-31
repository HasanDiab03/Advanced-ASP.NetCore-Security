	using Common.Requests;
using Common.Responses;
using Common.Responses.Wrappers;

namespace Application.Services.Identity
{
	public interface ITokenService
	{
		Task<ResponseWrapper<TokenResponse>> GetTokenAsync(TokenRequest request);
		Task<ResponseWrapper<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest request);
	}
}
