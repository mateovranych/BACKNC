using backnc.Common;
using backnc.Common.DTOs.AdministradorDTO;
using backnc.Common.DTOs.ClientesDTO;
using backnc.Common.Response;
using backnc.Data.POCOEntities;
using backnc.Interfaces;
using backnc.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backnc.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientesController : ControllerBase
	{
        private readonly IClienteService _clienteService;
        public ClientesController(IClienteService _clienteService)
        {
            this._clienteService = _clienteService;
        }


		[HttpGet]
		public async Task<IActionResult> GetAllClientes()
		{
			var clientes = await _clienteService.GetAllClientesAsync();
			return Ok(clientes);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetClienteById(int id)
		{
			var cliente = await _clienteService.GetClienteByIdAsync(id);
			if (cliente == null)
			{
				return NotFound("Cliente no encontrado");
			}
			return Ok(cliente);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCliente(CreateClienteDTO clienteDTO)
		{
		

			//var hashedPassword = PasswordHasher.HashPassword(clienteDTO.Password);

			//var user = new User
			//{
			//	username = clienteDTO.UserName,
			//	firstName = clienteDTO.FirstName,
			//	lastName = clienteDTO.LastName,
			//	dni = clienteDTO.dni,
			//	address = clienteDTO.address,
			//	phoneNumber = clienteDTO.phoneNumber,	
			//	email = clienteDTO.Email,
			//	password = hashedPassword 
			//};

			await _clienteService.CreateClienteAsync(new User(), clienteDTO);
			return Ok("Cliente creado con exito");
			
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCliente(int id, CreateClienteDTO adminDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingUser = await _clienteService.GetClienteByIdAsync(id);
			if (existingUser == null)
			{
				return NotFound("Cliente no encontrado");
			}

			existingUser.username = adminDto.UserName;
			existingUser.email = adminDto.Email;
			existingUser.firstName = adminDto.FirstName;
			existingUser.lastName = adminDto.LastName;
			existingUser.dni = adminDto.dni;
			existingUser.address = adminDto.address;
			existingUser.phoneNumber = adminDto.phoneNumber;			
			existingUser.password = PasswordHasher.HashPassword(adminDto.Password);

			await _clienteService.UpdateClienteAsync(existingUser);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCliente(int id)
		{
			var cliente = await _clienteService.GetClienteByIdAsync(id);
			if (cliente == null)
			{
				return NotFound("Cliente no encontrado");
			}

			await _clienteService.DeleteClienteAsync(id);
			return NoContent();
		}
	}
}
