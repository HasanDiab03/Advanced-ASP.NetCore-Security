using Application.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Infrastructure.Services.Identity
{
	public class RoleRepository : IRoleRepository
	{
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _context;

		public RoleRepository(RoleManager<ApplicationRole> roleManager,
			UserManager<ApplicationUser> userManager, IMapper mapper,
			ApplicationDbContext context)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_mapper = mapper;
			_context = context;
		}
		public async Task<IResponseWrapper> CreateRole(CreateRoleRequest request)
		{
			var roleExists = await _roleManager.FindByNameAsync(request.Name);
			if(roleExists is not null)
			{
				return ResponseWrapper<string>.Fail("Role Already Exists");
			}
			var appRole = new ApplicationRole { Name = request.Name, Description = request.Description };
			var identityResult = await _roleManager.CreateAsync(appRole);
			if(!identityResult.Succeeded)
			{
				return ResponseWrapper<string>.Fail(GetIdentityResultErrorDescription(identityResult));
			}
			return ResponseWrapper<string>.Success("Role Created Successfully");
		}

		public async Task<IResponseWrapper> DeleteRole(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role is null)
				return ResponseWrapper<string>.Fail("Role does not exist");
			if (role.Name == AppRoles.Admin)
				return ResponseWrapper<string>.Fail("Cannot delete Admin role");
			var allUsers = await _userManager.GetUsersInRoleAsync(role.Name);
			if(allUsers.Count > 0) 
			{
				return ResponseWrapper<string>.Fail("Role is currently assigned to a user");
			}
			var identityResult = await _roleManager.DeleteAsync(role);
			if(!identityResult.Succeeded)
			{
				return ResponseWrapper<string>.Fail(GetIdentityResultErrorDescription(identityResult));
			}
			return ResponseWrapper<string>.Success("Role deleted successfully!");
		}

		public async Task<IResponseWrapper> GetAllRoles()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			var mappedRoles = _mapper.Map<List<RoleResponse>>(roles);
			return ResponseWrapper<List<RoleResponse>>.Success(mappedRoles);
		}

		public async Task<IResponseWrapper> GetPermissions(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role is null)
				return ResponseWrapper<string>.Fail("Role does not exist");
			var allPermission = AppPermissions.AllPermission;
			var roleClaimResponse = new RoleClaimResponse
			{
				Role = new()
				{
					Id = id,
					Name = role.Name,
					Description = role.Description
				},
				RoleClaims = new()
			};
			var roleClaims = (await GetRoleClaims(id)).Select(x => x.ClaimValue).ToList();
			foreach(var permission in allPermission)
			{
				var roleClaimVM = new RoleClaimViewModel
				{
					RoleId = id,
					ClaimType = AppClaim.Permission,
					ClaimValue = permission.Name,
					Description = permission.Description,
					Group = permission.Group,
				};

				if(roleClaims.Contains(permission.Name))
				{
					roleClaimVM.IsAssignedToRole = true;
				}
				roleClaimResponse.RoleClaims.Add(roleClaimVM);
			}
			return ResponseWrapper<RoleClaimResponse>.Success(roleClaimResponse);
		}

		public async Task<IResponseWrapper> GetRole(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if(role is null)
			{
				return ResponseWrapper<string>.Fail("Role does not exist");
			}
			var mappedRole = _mapper.Map<RoleResponse>(role);
			return ResponseWrapper<RoleResponse>.Success(mappedRole);
		}

		public async Task<IResponseWrapper> UpdateRole(string id, UpdateRoleRequest request)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role is null)
				return ResponseWrapper<string>.Fail("Role does not exist");
			if (role.Name == AppRoles.Admin)
				return ResponseWrapper<string>.Fail("Cannot update Admin role");
			role.Name = request.Name;
			role.Description = request.Description;
			var identityResult = await _roleManager.UpdateAsync(role);
			if (!identityResult.Succeeded)
				return ResponseWrapper<string>.Fail(GetIdentityResultErrorDescription(identityResult));
			return ResponseWrapper<string>.Success("Role Updated Successfully");
		}
		public async Task<IResponseWrapper> UpdateRolePermissions(string id, UpdateRolePermissionsRequest request)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role is null)
				return ResponseWrapper<string>.Fail("Role does not exist");
			//if (role.Name == AppRoles.Admin)
				//return ResponseWrapper<string>.Fail("Cannot update permissions for this role");
			var permissionsToBeAssigned = request.RoleClaims.Where(x => x.IsAssignedToRole).ToList();
			var claimsToRemove = await _roleManager.GetClaimsAsync(role);
			foreach (var claim in claimsToRemove)
				await _roleManager.RemoveClaimAsync(role, claim);
            foreach (var claim in permissionsToBeAssigned)
            {
				claim.RoleId = id;
				var mappedClaim = _mapper.Map<ApplicationRoleClaim>(claim);
				await _context.RoleClaims.AddAsync(mappedClaim);
            }
			await _context.SaveChangesAsync();
			return ResponseWrapper<string>.Success("Role Claims Updated successfully");
		}

		private List<string> GetIdentityResultErrorDescription(IdentityResult identityResult)
			=> identityResult.Errors.Select(x => x.Description).ToList();
		private async Task<List<RoleClaimViewModel>> GetRoleClaims(string roleId)
		{
			var roleClaims = await _context.RoleClaims.Where(x => x.RoleId == roleId).ToListAsync();
			var mappedClaims = _mapper.Map<List<RoleClaimViewModel>>(roleClaims);
			return mappedClaims;
		}

	}
}
