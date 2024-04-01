using Application.Services.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Queries
{
	public record GetUserQuery(string Id) : IRequest<IResponseWrapper>;
	public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IResponseWrapper>
	{
		private readonly IUserRepository _userRepository;

		public GetUserQueryHandler(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public async Task<IResponseWrapper> Handle(GetUserQuery request, CancellationToken cancellationToken)
		{
			return await _userRepository.GetUserById(request.Id);
		}
	}
}
