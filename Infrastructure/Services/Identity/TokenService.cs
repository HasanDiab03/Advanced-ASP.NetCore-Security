using Application.AppConfigs;
using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services.Identity
{
    public class TokenService : ITokenService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly AppConfiguration _appConfiguration;
		public TokenService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
			IOptions<AppConfiguration> appConfiguration)
        {
			_userManager = userManager;
			_roleManager = roleManager;
			_appConfiguration = appConfiguration.Value;
		}
        public async Task<ResponseWrapper<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest request)
		{
			if (request is null)
				return ResponseWrapper<TokenResponse>.Fail("Invalid Token");
			var princiapl = GetPrincipalFromToken(request.Token);
			var userEmail = princiapl.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(userEmail);
			if(user is null)
				return ResponseWrapper<TokenResponse>.Fail("User not found");
			if(!user.RefreshToken.Equals(request.RefreshToken) || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
			{
				return ResponseWrapper<TokenResponse>.Fail("Invalid Token");
			}
			var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
			user.RefreshToken = GenerateRefreshToken();
			await _userManager.UpdateAsync(user);
			var response = new TokenResponse
			{
				Token = token,
				RefreshToken = user.RefreshToken,
				RefreshTokenExpiryTime = user.RefreshTokenExpiryDate
			};
			return ResponseWrapper<TokenResponse>.Success(response);
		}

		public async Task<ResponseWrapper<TokenResponse>> GetTokenAsync(TokenRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if(user is null)
			{
				return ResponseWrapper<TokenResponse>.Fail("Invalid Credentials");
			}
			if(!user.IsActive)
			{
				return ResponseWrapper<TokenResponse>.Fail("User is not active. Please contact the administrator");
			}
			if(!user.EmailConfirmed)
			{
				return ResponseWrapper<TokenResponse>.Fail("Email Not Cofirmed");
			}
			var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
			if (!isPasswordValid)
			{
				return ResponseWrapper<TokenResponse>.Fail("Invalid Credentials");
			}
			// generate Refresh Token
			user.RefreshToken = GenerateRefreshToken();
			user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7);
			await _userManager.UpdateAsync(user);
			// generate JWt
			var token = await GenerateJWTAsync(user);
			var response = new TokenResponse
			{
				Token = token,
				RefreshToken = user.RefreshToken,
				RefreshTokenExpiryTime = user.RefreshTokenExpiryDate
			};
			return ResponseWrapper<TokenResponse>.Success(response);
		}
		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var rnd = RandomNumberGenerator.Create();
			rnd.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
		private async Task<string> GenerateJWTAsync(ApplicationUser user)
		{
			return GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
		}
		private string GenerateEncryptedToken(SigningCredentials credentials, IEnumerable<Claim> claims)
		{
			var token = new JwtSecurityToken(
				claims: claims,
				signingCredentials: credentials,
				expires: DateTime.UtcNow.AddMinutes(_appConfiguration.TokenExpiryInMinutes));
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(token);
		}
		private SigningCredentials GetSigningCredentials()
		{
			var secret = Encoding.UTF8.GetBytes(_appConfiguration.Secret);
			return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
		}
		private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();
			var permissionClaims = new List<Claim>();
			foreach (var role in roles)
			{
				roleClaims.Add(new Claim(ClaimTypes.Role, role));
				var currentRole = await _roleManager.FindByNameAsync(role);
				var allRolePermissions = await _roleManager.GetClaimsAsync(currentRole);
				permissionClaims.AddRange(allRolePermissions);
			}
			var claims = new List<Claim> 
			{
				new (ClaimTypes.NameIdentifier, user.Id),
				new (ClaimTypes.Email, user.Email),
				new (ClaimTypes.Name, user.FirstName),
				new (ClaimTypes.Surname, user.LastName),
				new (ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
			}
			.Union(userClaims)
			.Union(roleClaims)
			.Union(permissionClaims);
			return claims;
		}
		private ClaimsPrincipal GetPrincipalFromToken(string token)
		{
			var tokenValidationParams = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Secret)),
				ValidateIssuer = false,
				ValidateAudience = false,
				RoleClaimType = ClaimTypes.Role,
				ClockSkew = TimeSpan.Zero
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out var securityToken);
			if(securityToken is not JwtSecurityToken jwtSecurityToken ||
				!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid Token");
			}
			return principal;
		}
	}
}
