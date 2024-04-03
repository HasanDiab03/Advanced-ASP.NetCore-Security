using Application.Services.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Queries
{
	public record GetRoleByIdQuery(string Id) : IRequest<IResponseWrapper>;
	public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, IResponseWrapper>
	{
		private readonly IRoleRepository _roleRepository;

		public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
        {
			_roleRepository = roleRepository;
		}
        public async Task<IResponseWrapper> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
		{
			return await _roleRepository.GetRole(request.Id);
		}
	}
}
