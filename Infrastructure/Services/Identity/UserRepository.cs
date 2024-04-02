using Application.AppConfigs;
using Application.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Identity
{
	public class UserRepository : IUserRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly IMapper _mapper;
		private readonly ICurrentUserRepository _currentUserRepository;

		public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
			IMapper mapper, ICurrentUserRepository currentUserRepository)
        {
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
			_currentUserRepository = currentUserRepository;
		}

		public async Task<IResponseWrapper> ChangePassword(string id, ChangePasswordRequest request)
		{
			var user = await _userManager.FindByIdAsync(id);
			if(user is null)
			{
				return ResponseWrapper<string>.Fail("User does not exist");
			}
			var identityResult = await _userManager
				.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
			if(!identityResult.Succeeded)
			{
				return ResponseWrapper<string>.Fail(GetIdentityResultErrorDescription(identityResult));
			}
			return ResponseWrapper<string>.Success("Password has been updated!");
		}

		public async Task<IResponseWrapper> ChangeUserStatus(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if(user is null)
			{
				return ResponseWrapper<string>.Fail("User does not exist");
			}
			user.IsActive = !user.IsActive;
			var identityResult = await _userManager.UpdateAsync(user);
			if(!identityResult.Succeeded)
			{
				return ResponseWrapper<string>.Fail(GetIdentityResultErrorDescription(identityResult));
			}
			return ResponseWrapper<string>.Success($"Status Has been updated, user is now: {(user.IsActive ? "Active" : "InActive")}");
		}

		public async Task<IResponseWrapper> GetAllUsers()
		{
			var users = await _userManager.Users.ToListAsync();
			if(users is null)
			{
				return ResponseWrapper<string>.Fail("Failed to fetch users");
			}
			var mappedUsers = _mapper.Map<List<UserResponse>>(users);
			return ResponseWrapper<List<UserResponse>>.Success(mappedUsers);
		}

		public async Task<IResponseWrapper> GetRoles(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if(user is null)
			{
				return ResponseWrapper<string>.Fail("User does not exist");
			}
			var allRoles = await _roleManager.Roles.ToListAsync();
			var userRoles = await _userManager.GetRolesAsync(user);
			var userRolesVM = allRoles.Select(role => new UserRoleViewModel
			{
				RoleName = role.Name,
				RoleDescription = role.Description,
				IsAssignedToUser = userRoles.Contains(role.Name)
			}).ToList();
			
			return ResponseWrapper<List<UserRoleViewModel>>.Success(userRolesVM);
		}

		public async Task<IResponseWrapper> GetUserById(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if(user is null)
			{
				return ResponseWrapper<string>.Fail("User does not exist");
			}
			var mappedUser = _mapper.Map<UserResponse>(user);
			return ResponseWrapper<UserResponse>.Success(mappedUser);
		}

		public async Task<IResponseWrapper> RegisterUser(RegisterRequest request)
		{
			var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
			if(userWithSameEmail is not null)
			{
				return ResponseWrapper<string>.Fail("User with same email already exists");
			}

			var userWithSameUsername = await _userManager.FindByNameAsync(request.Username);
			if(userWithSameUsername is not null)
			{
				return ResponseWrapper<string>.Fail("User with same username already exists");
			}

			var user = new ApplicationUser
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				IsActive = request.ActivateUser,
				UserName = request.Username,
				EmailConfirmed = request.AutoConfirmEmail,
				PhoneNumber = request.PhoneNumber
			};
			var identityResult = await _userManager.CreateAsync(user, request.Password);
			if(identityResult.Succeeded)
			{
				// Assign To Role
				await _userManager.AddToRoleAsync(user, AppRoles.Basic);
				return ResponseWrapper<string>.Success("User Registered Successfully");
			}
			return ResponseWrapper<string>.Fail(GetIdentityResultErrorDescription(identityResult));
		}

		public async Task<IResponseWrapper> UpdateUser(string id, UpdateUserRequest request)
		{
			var user = await _userManager.FindByIdAsync(id);
			if(user is null)
			{
				return ResponseWrapper<string>.Fail("User does not exist");
			}
			_mapper.Map(request, user);
			var identityResult = await _userManager.UpdateAsync(user);
			if(!identityResult.Succeeded)
			{
				return ResponseWrapper<string>.Fail(GetIdentityResultErrorDescription(identityResult));
			}
			var updateUserMap = _mapper.Map<UserResponse>(user);
			return ResponseWrapper<UserResponse>.Success(updateUserMap);
		}

		public async Task<IResponseWrapper> UpdateUserRoles(string id, UpdateUserRolesRequest request)
		{
			var user = await _userManager.FindByIdAsync(id);
			if(user is null)
			{
				return ResponseWrapper<string>.Fail("User not found");
			}
			if(user.Email == AppCredentials.Email)
			{
				return ResponseWrapper<string>.Fail("User Roles Update Not Permitted");
			}
			var roles = await _userManager.GetRolesAsync(user);
			var assignedRoles = request.Roles.Where(x => x.IsAssignedToUser).ToList();
			var currentLoggedInUser = await _userManager.FindByIdAsync(_currentUserRepository.Id);
			if(currentLoggedInUser is null)
			{
				return ResponseWrapper<string>.Fail("User does not exist");
			} 
			if(!(await _userManager.IsInRoleAsync(currentLoggedInUser, AppRoles.Admin)))
			{
				return ResponseWrapper<string>.Fail("User Roles Update Not Permitted");
			}
			// approach => remove roles that have IsAssignedToUser = false, add ones that have it true
			var identityResult = await _userManager.RemoveFromRolesAsync(user, roles);
			if(!identityResult.Succeeded)
			{
				return ResponseWrapper<string>.Fail(GetIdentityResultErrorDescription(identityResult));
			}
			identityResult = await _userManager.AddToRolesAsync(user, assignedRoles.Select(x => x.RoleName));
			if(!identityResult.Succeeded)
			{
				return ResponseWrapper<string>.Fail(GetIdentityResultErrorDescription(identityResult));
			}
			return ResponseWrapper<string>.Success("User Roles Update Successfully");
				
		}

		private List<string> GetIdentityResultErrorDescription(IdentityResult identityResult)
		{
			return identityResult.Errors.Select(x => x.Description).ToList();
		}
	}
}
