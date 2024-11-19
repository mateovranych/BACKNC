using backnc.Common;
using backnc.Common.DTOs.AdministradorDTO;
using backnc.Data.POCOEntities;
using backnc.Interfaces;
using backnc.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backnc.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdministradorController : ControllerBase
	{
		private readonly IAdministradorService _administradorService;
		
        public AdministradorController(IAdministradorService administradorService)
        {
			this._administradorService = administradorService;         
        }

		
		[HttpGet]
		public async Task<IActionResult> GetAllAdmins()
		{
			var clientes = await _administradorService.GetAllAdminsAsync();
			return Ok(clientes);
		}		

		[HttpGet("{id}")]
		public async Task<IActionResult> GetAdminById(int id)
		{
			var cliente = await _administradorService.GetAdminByIdAsync(id);
			if (cliente == null)
			{
				return NotFound("Administrador no encontrado");
			}
			return Ok(cliente);
		}

		

		[HttpPost]
		public async Task<IActionResult> CreateAdmin(CreateAdministradorDTO adminDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}			
		
			var nuevoAdministrador = await _administradorService.CreateAdminAsync(adminDto);
			return CreatedAtAction(nameof(GetAdminById), new {id = nuevoAdministrador.Id}, nuevoAdministrador);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAdmin(int id, UpdateAdministradorDTO adminDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await _administradorService.UpdateAdminAsync(id,adminDto);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAdmin(int id)
		{
			var cliente = await _administradorService.GetAdminByIdAsync(id);
			if (cliente == null)
			{
				return NotFound("Administrador no encontrado");
			}

			await _administradorService.DeleteAdminAsync(id);
			return NoContent();
		}
	}
}
