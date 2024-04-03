using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Commands
{
	public record CreateRoleCommand(CreateRoleRequest Request) : IRequest<IResponseWrapper>;
	public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, IResponseWrapper>
	{
		private readonly IRoleRepository _roleRepository;

		public CreateRoleCommandHandler(IRoleRepository roleRepository)
        {
			_roleRepository = roleRepository;
		}
        public async Task<IResponseWrapper> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
		{
			return await _roleRepository.CreateRole(request.Request);
		}
	}
}
