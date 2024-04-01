using Application.Services.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Commands
{
	public record ChangeUserStatusCommand(string Id) : IRequest<IResponseWrapper>;
	public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, IResponseWrapper>
	{
		private readonly IUserRepository _userRepository;

		public ChangeUserStatusCommandHandler(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public async Task<IResponseWrapper> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
		{
			return await _userRepository.ChangeUserStatus(request.Id);
		}
	}
}
