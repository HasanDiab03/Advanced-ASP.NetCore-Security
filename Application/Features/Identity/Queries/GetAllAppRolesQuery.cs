using Application.Services.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Queries
{
	public record GetAllAppRolesQuery : IRequest<IResponseWrapper>;
	public class GetAllAppRolesQueryHandler : IRequestHandler<GetAllAppRolesQuery, IResponseWrapper>
	{
		private readonly IRoleRepository _roleRepository;

		public GetAllAppRolesQueryHandler(IRoleRepository roleRepository)
        {
			_roleRepository = roleRepository;
		}
        public async Task<IResponseWrapper> Handle(GetAllAppRolesQuery request, CancellationToken cancellationToken)
		{
			return await _roleRepository.GetAllRoles();
		}
	}
}
