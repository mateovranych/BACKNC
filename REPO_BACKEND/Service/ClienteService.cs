using backnc.Common.Response;
using backnc.Data.Interface;
using backnc.Common.DTOs.ClientesDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using backnc.Data.POCOEntities;
using backnc.Interfaces;
using backnc.Common;

namespace backnc.Service
{
	public class ClienteService : IClienteService
	{
        private readonly IAppDbContext _appDbContext;        
        public ClienteService(IAppDbContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }

		public async Task<IEnumerable<User>> GetAllClientesAsync()
		{
			return await _appDbContext.Users
				.Include(u => u.UserRoles)
				.Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Cliente"))
				.ToListAsync();
		}

		public async Task<User> GetClienteByIdAsync(int id)
		{
			return await _appDbContext.Users
				.Include(u => u.UserRoles)
				.FirstOrDefaultAsync(u => u.Id == id && u.UserRoles.Any(ur => ur.Role.Name == "Cliente"));
		}

		public async Task CreateClienteAsync(User user, CreateClienteDTO clienteDTO)
		{
			var hashedPassword = PasswordHasher.HashPassword(clienteDTO.Password);

			var nuevoUser = new User
			{
				username = clienteDTO.UserName,
				firstName = clienteDTO.FirstName,
				lastName = clienteDTO.LastName,
				dni = clienteDTO.dni,
				address = clienteDTO.address,
				phoneNumber = clienteDTO.phoneNumber,
				email = clienteDTO.Email,
				password = hashedPassword
			};

			_appDbContext.Users.Add(nuevoUser);
			await _appDbContext.SaveChangesAsync();

			var profile = new Profile
			{
				UserId = nuevoUser.Id,				
				Specialty = "",
				Experience = "",
				Description = "",
				ImageUrl = ""
			};
			_appDbContext.Profiles.Add(profile);
			await _appDbContext.SaveChangesAsync();

			var role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Cliente");
			var userRole = new UserRole
			{
				UserId = nuevoUser.Id,
				RoleId = role.Id
			};

			_appDbContext.UserRoles.Add(userRole);
			await _appDbContext.SaveChangesAsync();
		}

		public async Task UpdateClienteAsync(User user)
		{
			_appDbContext.Users.Update(user);
			await _appDbContext.SaveChangesAsync();
		}

		public async Task DeleteClienteAsync(int id)
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
