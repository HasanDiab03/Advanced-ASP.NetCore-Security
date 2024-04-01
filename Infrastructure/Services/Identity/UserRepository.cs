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

		public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
			IMapper mapper)
        {
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
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
				return ResponseWrapper<string>.Fail("Failed To update password");
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
				return ResponseWrapper<string>.Fail("Failed to update status");
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
			return ResponseWrapper<string>.Fail("User Registration Failed");
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
				return ResponseWrapper<string>.Fail("Failed to update user");
			}
			var updateUserMap = _mapper.Map<UserResponse>(user);
			return ResponseWrapper<UserResponse>.Success(updateUserMap);
		}
	}
}
