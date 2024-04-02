using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Commands
{
	public record UpateUserRolesCommand(string Id, UpdateUserRolesRequest Request) : IRequest<IResponseWrapper>;
	public class UpateUserRolesCommandHandler : IRequestHandler<UpateUserRolesCommand, IResponseWrapper>
	{
		private readonly IUserRepository _userRepository;

		public UpateUserRolesCommandHandler(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public async Task<IResponseWrapper> Handle(UpateUserRolesCommand request, CancellationToken cancellationToken)
		{
			return await _userRepository.UpdateUserRoles(request.Id, request.Request);
		}
	}
}
