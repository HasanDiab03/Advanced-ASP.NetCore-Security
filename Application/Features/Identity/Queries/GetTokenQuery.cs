using Application.Services.Identity;
using Common.Requests;
using Common.Responses;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Queries
{
	public record GetTokenQuery(TokenRequest Request) : IRequest<ResponseWrapper<TokenResponse>>;
	public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, ResponseWrapper<TokenResponse>>
	{
		private readonly ITokenService _tokenService;

		public GetTokenQueryHandler(ITokenService tokenService)
        {
			_tokenService = tokenService;
		}
        public async Task<ResponseWrapper<TokenResponse>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
		{
			return await _tokenService.GetTokenAsync(request.Request);
		}
	}
}
