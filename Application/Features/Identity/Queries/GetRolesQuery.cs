using Application.Services.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Queries
{
	public record GetRolesQuery(string Id) : IRequest<IResponseWrapper>;
	public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IResponseWrapper>
	{
		private readonly IUserRepository _userRepository;

		public GetRolesQueryHandler(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public async Task<IResponseWrapper> Handle(GetRolesQuery request, CancellationToken cancellationToken)
		{
			return await _userRepository.GetRoles(request.Id);
		}
	}
}
