using Application.Pipelines;
using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Commands
{
	public record RegisterUserCommand(RegisterRequest Request) : 
		IRequest<IResponseWrapper>, IValidateMe;
	public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IResponseWrapper>
	{
		private readonly IUserRepository _userRepository;

		public RegisterUserCommandHandler(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public async Task<IResponseWrapper> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			return await _userRepository.RegisterUser(request.Request);
		}
	}
}
