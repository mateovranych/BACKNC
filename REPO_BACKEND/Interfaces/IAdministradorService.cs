using backnc.Common.DTOs.AdministradorDTO;
using backnc.Data.POCOEntities;

namespace backnc.Interfaces
{
	public interface IAdministradorService
	{
		public Task<IEnumerable<AdministradorDTO>> GetAllAdminsAsync();

		public Task<AdministradorDTO> GetAdminByIdAsync(int id);

		public Task<UpdateAdministradorDTO> GetAdminByIdAsyncWithPass(int id);

		public Task<User> CreateAdminAsync(CreateAdministradorDTO adminDTO);

		public Task<bool> UpdateAdminAsync(int id, UpdateAdministradorDTO adminDto);

		public Task DeleteAdminAsync(int id);

	}
}
