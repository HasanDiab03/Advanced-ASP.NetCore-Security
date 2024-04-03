using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Commands
{
	public record UpdateRoleCommand(string Id, UpdateRoleRequest Request) : IRequest<IResponseWrapper>;
	public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, IResponseWrapper>
	{
		private readonly IRoleRepository _roleRepository;

		public UpdateRoleCommandHandler(IRoleRepository roleRepository)
        {
			_roleRepository = roleRepository;
		}
        public async Task<IResponseWrapper> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
		{
			return await _roleRepository.UpdateRole(request.Id, request.Request);
		}
	}
}
