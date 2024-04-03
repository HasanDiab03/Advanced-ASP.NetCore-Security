using Application.Services.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Commands
{
	public record DeleteRoleCommand(string Id) : IRequest<IResponseWrapper>;
	public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, IResponseWrapper>
	{
		private readonly IRoleRepository _roleRepository;

		public DeleteRoleCommandHandler(IRoleRepository roleRepository)
        {
			_roleRepository = roleRepository;
		}
        public async Task<IResponseWrapper> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
		{
			return await _roleRepository.DeleteRole(request.Id);
		}
	}
}
