using Application.Services.Identity;
using Common.Requests;
using Common.Responses;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Queries
{
	public record GetRefreshTokenQuery(RefreshTokenRequest Request) : IRequest<ResponseWrapper<TokenResponse>>;
	public class GetRefreshTokenQueryHandler : IRequestHandler<GetRefreshTokenQuery, ResponseWrapper<TokenResponse>>
	{
		private readonly ITokenService _tokenService;

		public GetRefreshTokenQueryHandler(ITokenService tokenService)
        {
			_tokenService = tokenService;
		}
        public async Task<ResponseWrapper<TokenResponse>> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
		{
			return await _tokenService.GetRefreshTokenAsync(request.Request);
		}
	}
}
