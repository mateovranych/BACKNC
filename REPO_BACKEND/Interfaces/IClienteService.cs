using backnc.Common.DTOs.ClientesDTO;
using backnc.Data.POCOEntities;

namespace backnc.Interfaces
{
	public interface IClienteService
	{
		public Task<IEnumerable<User>> GetAllClientesAsync();

		public Task<User> GetClienteByIdAsync(int id);

		//public Task CreateClienteAsync(User user);

		public Task CreateClienteAsync(User user, CreateClienteDTO clienteDTO);

		public Task UpdateClienteAsync(User user);

		public Task DeleteClienteAsync(int id);
	}
}
