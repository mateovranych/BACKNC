using backnc.Data.Interface;
using Microsoft.EntityFrameworkCore;
using backnc.Data.POCOEntities;
using backnc.Common.DTOs.AdministradorDTO;
using backnc.Common;
using backnc.Interfaces;

namespace backnc.Service
{
	public class AdministradorService : IAdministradorService
	{
		private readonly IAppDbContext _appDbContext;

		public AdministradorService(IAppDbContext _appDbContext)
		{
			this._appDbContext = _appDbContext;
		}

		public async Task<IEnumerable<AdministradorDTO>> GetAllAdminsAsync()
		{
			return await _appDbContext.Users
				.Include(u => u.UserRoles)
				.Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Admin"))
				.Select(x => new AdministradorDTO
				{
					Id = x.Id.ToString(),		
					Username = x.username,
					Email = x.email,					
				}).ToListAsync();
		}

		public async Task<AdministradorDTO> GetAdminByIdAsync(int id)
		{
			return await _appDbContext.Users
				.Include(u => u.UserRoles)
				.Where(x => x.Id == id && x.UserRoles.Any(zx => zx.Role.Name == "Admin"))
				.Select(x => new AdministradorDTO
				{
					Id = x.Id.ToString(),
					Username = x.username,
					Email = x.email,
				}).FirstOrDefaultAsync();				
		}

		public async Task<UpdateAdministradorDTO> GetAdminByIdAsyncWithPass(int id)
		{
			return await _appDbContext.Users
				.Include(u => u.UserRoles)
				.Where(x => x.Id == id && x.UserRoles.Any(zx => zx.Role.Name == "Admin"))
				.Select(x => new UpdateAdministradorDTO
				{					
					Username = x.username,
					Email = x.email,
					Password = x.password,
				}).FirstOrDefaultAsync();
		}

		//public async Task CreateAdminAsync(User user)
		//{
		//	_appDbContext.Users.Add(user);
		//	await _appDbContext.SaveChangesAsync();
			
		//	var role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
		//	var userRole = new UserRole
		//	{
		//		UserId = user.Id,
		//		RoleId = role.Id
		//	};

		//	_appDbContext.UserRoles.Add(userRole);
		//	await _appDbContext.SaveChangesAsync();
		//}
		public async Task<User> CreateAdminAsync(CreateAdministradorDTO adminDTO)
		{
			var HasheoDeContraseña = PasswordHasher.HashPassword(adminDTO.Password);

			var user = new User
			{
				username = adminDTO.Username,
				email = adminDTO.Email,
				password = HasheoDeContraseña,
			};

			await _appDbContext.Users.AddAsync(user);
			await _appDbContext.SaveChangesAsync();

			var RolAdministrador = await _appDbContext.Roles.FirstOrDefaultAsync(x => x.Name == "Admin");
			if (RolAdministrador != null)
			{
				var userRole = new UserRole
				{
					UserId = user.Id,
					RoleId = RolAdministrador.Id,
				};

				await _appDbContext.UserRoles.AddAsync(userRole);
				await _appDbContext.SaveChangesAsync();
			}
		
			return user;
		}

		public async Task<bool> UpdateAdminAsync(int id, UpdateAdministradorDTO adminDto)
		{
			var existeAdministrador = await _appDbContext.Users.Include(x => x.UserRoles)
				.FirstOrDefaultAsync(x => x.Id == id && x.UserRoles.Any(zx => zx.Role.Name == "Admin"));
			if (existeAdministrador == null) 
			{ 
				return false; 
			}
			existeAdministrador.username = adminDto.Username;
			existeAdministrador.email = adminDto.Email;

			if (!string.IsNullOrEmpty(adminDto.Password))
			{
				existeAdministrador.password = PasswordHasher.HashPassword(adminDto.Password);
			}
						
			_appDbContext.Users.Update(existeAdministrador);
			await _appDbContext.SaveChangesAsync();
			return true;
		}

		public async Task DeleteAdminAsync(int id)
		{
			var user = await _appDbContext.Users.FindAsync(id);
			if (user != null)
			{
				_appDbContext.Users.Remove(user);
				await _appDbContext.SaveChangesAsync();
			}
		}

	}
}
