using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Commands
{
	public record ChangePasswordCommand(string Id, ChangePasswordRequest Request) : IRequest<IResponseWrapper>;
	public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IResponseWrapper>
	{
		private readonly IUserRepository _userRepository;

		public ChangePasswordCommandHandler(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public async Task<IResponseWrapper> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
		{
			return await _userRepository.ChangePassword(request.Id, request.Request);
		}
	}
}
