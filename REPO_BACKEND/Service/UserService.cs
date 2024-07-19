﻿using backnc.Common;
using backnc.Common.Response;
using backnc.Data.ConfigEntities;
using backnc.Data.Interface;
using backnc.Data.POCOEntities;
using backnc.Interfaces;
using backnc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backnc.Service
{
    public class UserService : IUserService
    {
        private readonly IAppDbContext _context;
		private readonly IUserValidationService _userValidationService;
		private readonly IConfiguration _configuration;
        public UserService(IAppDbContext context,IConfiguration configuration, IUserValidationService userValidationService)
        {
            _context = context;
            _configuration = configuration;
			_userValidationService = userValidationService;

		}
		public async Task<BaseResponse> Authenticate(LoginUser userLogin)
		{
			var user = await _context.Users
				.Where(u => u.UserName == userLogin.UserName)
				.FirstOrDefaultAsync();

			if (user == null)
			{
				return Response.ValidationError("Usuario no encontrado", new List<string> { "El usuario no existe o las credenciales son incorrectas." });
			}

			var hashedPassword = PasswordHasher.HashPassword(userLogin.Password);
			if (user.Password != hashedPassword)
			{
				return Response.ValidationError("Credenciales incorrectas", new List<string> { "El usuario no existe o las credenciales son incorrectas." });
			}

			var token = Generate(user);
			return Response.Success(token);
		}
		public async Task<BaseResponse> Register(RegisterUser registerUser)
		{
			var validator = new RegisterUserValidator(_userValidationService);
			var validationResult = await validator.ValidateAsync(registerUser);

			if (!validationResult.IsValid)
			{
				return Response.ValidationError("Error de validación", validationResult.Errors.Select(e => e.ErrorMessage).ToList());
			}

			var user = new User
			{
				UserName = registerUser.userName,
				firstName = registerUser.firstName,
				lastName = registerUser.lastName,
				email = registerUser.email,
				dni = registerUser.dni,
				address = registerUser.address,
				phoneNumber = registerUser.phoneNumber,
				Password = PasswordHasher.HashPassword(registerUser.password)
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Cliente");
			if (role == null)
			{
				return Response.ValidationError("Rol no encontrado", new List<string> { "El rol 'Cliente' no existe." });
			}
			var userRole = new UserRole
			{
				UserId = user.Id,
				RoleId = role.Id
			};
			_context.UserRoles.Add(userRole);
			await _context.SaveChangesAsync();
			return Response.Success("Usuario registrado exitosamente");
		}

		private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var rol = _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.Role.Name)       
            .FirstOrDefault();
			
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Role,rol)
            };
            
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
