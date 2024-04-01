using Application.Services.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Queries
{
	public record GetUsersQuery : IRequest<IResponseWrapper>;
	public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IResponseWrapper>
	{
		private readonly IUserRepository _userRepository;

		public GetUsersQueryHandler(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public async Task<IResponseWrapper> Handle(GetUsersQuery request, CancellationToken cancellationToken)
		{
			return await _userRepository.GetAllUsers();
		}
	}
}
