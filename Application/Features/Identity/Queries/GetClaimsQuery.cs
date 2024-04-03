using Application.Services.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Queries
{
	public record GetClaimsQuery(string Id) : IRequest<IResponseWrapper>;
	public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IResponseWrapper>
	{
		private readonly IRoleRepository _roleRepository;

		public GetClaimsQueryHandler(IRoleRepository roleRepository)
        {
			_roleRepository = roleRepository;
		}
        public async Task<IResponseWrapper> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
		{
			return await _roleRepository.GetPermissions(request.Id);
		}
	}
}
