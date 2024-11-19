using backnc.Common.Response;
using backnc.Data.Context;
using backnc.Data.Interface;
using backnc.Data.POCOEntities;
using backnc.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backnc.Service
{
	public class TokenService : ITokenService
	{
		private readonly IAppDbContext context;
		private readonly IConfiguration configuration;

		public TokenService(IAppDbContext context, IConfiguration configuration)
        {
			this.context = context;
			this.configuration = configuration;             
        }

        public string GenerateToken(User user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var rol = context.UserRoles
			.Where(ur => ur.UserId == user.Id)
			.Select(ur => ur.Role.Name)
			.FirstOrDefault();

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Role,rol),
			};

			var token = new JwtSecurityToken(
				configuration["Jwt:Issuer"],
				configuration["Jwt:Audience"],
				claims,
				expires: DateTime.Now.AddMinutes(15),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<BaseResponse> ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = configuration["Jwt:Issuer"],
					ValidateAudience = true,
					ValidAudience = configuration["Jwt:Audience"],
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
				var role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;

				var user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

				if (user == null)
				{
					return Response.ValidationError("Usuario no encontrado", new List<string> { "El token es válido pero el usuario no existe." });
				}

				var userResponse = new
				{
					UserName = user.username,
					Role = role,
				};

				return Response.Success(userResponse);
			}
			catch (Exception)
			{
				return Response.ValidationError("Token inválido", new List<string> { "El token es inválido o ha expirado." });
			}
		}
	}
}
