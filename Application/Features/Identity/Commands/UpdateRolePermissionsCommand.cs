using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Commands
{
	public record UpdateRolePermissionsCommand(string Id, UpdateRolePermissionsRequest Request) : IRequest<IResponseWrapper>;
	public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, IResponseWrapper>
	{
		private readonly IRoleRepository _roleRepository;

		public UpdateRolePermissionsCommandHandler(IRoleRepository roleRepository)
        {
			_roleRepository = roleRepository;
		}
        public async Task<IResponseWrapper> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
		{
			return await _roleRepository.UpdateRolePermissions(request.Id, request.Request);
		}
	}
}
