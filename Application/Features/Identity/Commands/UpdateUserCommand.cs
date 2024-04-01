using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Commands
{
	public record UpdateUserCommand(string Id, UpdateUserRequest Request) : IRequest<IResponseWrapper>;
	public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IResponseWrapper>
	{
		private readonly IUserRepository _userRepository;

		public UpdateUserCommandHandler(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public async Task<IResponseWrapper> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
		{
			return await _userRepository.UpdateUser(request.Id, request.Request);
		}
	}
}
